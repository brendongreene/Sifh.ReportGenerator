using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using OfficeOpenXml;

namespace Sifh.ReportGenerator.Core
{
    public interface ISchema { }
    public abstract class Importer<T> where T : ISchema
    {
        private DataTable _csvData;
        protected FileInfo SourceFile;
        private DataTable _fileData;
        private readonly List<List<string>> _multipleAliasColumns = new List<List<string>>();
        private readonly List<string> _optionalColumns = new List<string>();


        public List<string> Columns = new List<string>();
        public delegate void NotificationDel(object sender, string message);

        public delegate void DataExtractionDel(object sender, IEnumerable<T> records);
        public event NotificationDel Notification;
        public event DataExtractionDel DataExtraction;

        protected virtual int StartAtRow
        {
            get { return 1; }
        }

        public virtual int Import(FileInfo file, bool saveToDatabase = false)
        {
            SourceFileInfo = file;

            switch (file.Extension)
            {
                case ".csv":
                    return ImportCsv(file);
                case ".xlsx":
                case ".xls":
                    return ImportXls(file);
                default:
                    throw new Exception(string.Format("Invalid File Extension:{0}", file.Extension));
            }
        }

        public virtual IEnumerable<T> ExtractData(FileInfo file)
        {
            SourceFileInfo = file;

            switch (file.Extension)
            {
                case ".csv":
                    return ExtractCsvData(file);
                case ".xlsx":
                case ".xls":
                    return ExtractXlsData(file);
                default:
                    throw new Exception(string.Format("Invalid File Extension:{0}", file.Extension));
            }
        }

        public FileInfo SourceFileInfo { get; set; }

        public virtual bool HasHeaderRows
        {
            get { return true; }
        }

        public virtual int ImportXls(FileInfo file)
        {
            var records = ExtractXlsData(file);
            OnDataExtraction(records);
            return ImportToDataBase(records);
        }

        private IEnumerable<T> ExtractXlsData(FileInfo file)
        {
            _fileData = Utils.ParseExcelToDataTable(file, HasHeaderRows, StartAtRow);
            if (_fileData == null)
                return null;

            if (_fileData.Rows.Count == 0)
            {
                return null;
             
            }

            UpdateColumns(_fileData);


            UpdateDataTableColumns(_fileData);

            
            var records = MapToSchemaEntity<T>(_fileData);
            return records;
        }

        protected virtual DataTable ReadXls(FileInfo file)
        {
            return _fileData = Utils.ParseExcelToDataTable(file, HasHeaderRows, StartAtRow);
        }

        public virtual int ImportCsv(FileInfo file)
        {
            var records = ExtractCsvData(file);
            OnDataExtraction(records);
            return ImportToDataBase(records);
        }

        private IEnumerable<T> ExtractCsvData(FileInfo file)
        {
            SourceFile = file;
            _csvData = ReadCsv(file);

           
            if (_csvData.Rows.Count == 0)
            {
                throw new Exception("No records found.");
            }

            UpdateColumns(_csvData);

               var records = MapToSchemaEntity<T>(_csvData);
            return records;
        }

        protected virtual DataTable ReadCsv(FileInfo file)
        {
            using (var stream = new StreamReader(file.FullName))
            {
                var csvHelper = new CsvHelper.CsvReader(stream,CultureInfo.InvariantCulture);
                var dt = new DataTable();


                while (csvHelper.Read())
                {
                    if (csvHelper.Configuration.HasHeaderRecord && csvHelper.HeaderRecord == null)
                    {
                        csvHelper.ReadHeader();
                        continue;
                    }
                    if (dt.Columns.Count == 0 && csvHelper.HeaderRecord != null)
                    {
                        foreach (var header in csvHelper.HeaderRecord)
                        {
                            dt.Columns.Add(header);
                        }
                    }

                    var row = dt.NewRow();
                    foreach (DataColumn column in dt.Columns)
                    {
                        row[column.ColumnName] = csvHelper.GetField(column.DataType, column.ColumnName);
                    }
                    dt.Rows.Add(row);
                }
                return dt;
            }
        }

        protected abstract int ImportToDataBase(IEnumerable<T> records);

        private static IEnumerable<T> MapToSchemaEntity<T>(DataTable dataTable)
        {
            var type = typeof(T);

            var properties = type.GetProperties();
            var entityNamePropertyMapping =
                new Dictionary<string, PropertyInfo>(StringComparer.InvariantCultureIgnoreCase);
            var requiredProperties = new List<string>();

            foreach (var property in properties)
            {
                var aliasName =
                    property.GetCustomAttributes(false)
                        .FirstOrDefault(x => x.GetType() == typeof(MappingNameAttribute));

                //var aliasName = property.GetCustomAttribute(typeof(MappingNameAttribute));
                var aliases = aliasName == null
                    ? new List<string> { property.Name }
                    : ((MappingNameAttribute)aliasName).Names;

                //property.GetCustomAttribute(typeof(RequiredAttribute));
                //var requiredProperty = property.GetCustomAttributes(false).FirstOrDefault(x => x.GetType() == typeof (RequiredAttribute));

                foreach (var alias in aliases)
                {
                    entityNamePropertyMapping.Add(alias, property);
                }
            }

            var results = new List<T>();
            var dataTableColumns = (from DataColumn column in dataTable.Columns select column.ColumnName).ToList();
            foreach (DataRow row in dataTable.Rows)
            {
                var newRow = Activator.CreateInstance<T>();
                bool columnAdded = false;

                foreach (var column in dataTableColumns)
                {

                    var columnValue =
                        row[column].ToString()
                            .Replace("\"", string.Empty)
                            .Replace("(", string.Empty)
                            .Replace(")", string.Empty)
                            .Trim();


                    //Eliminate blank lines
                    if (string.IsNullOrEmpty(columnValue)) continue;

                    //Remove empty single quotes. Some empty columns contain "''"
                    if (columnValue == "''")
                        columnValue = columnValue.Replace("'", string.Empty);

                    try
                    {
                        if (!entityNamePropertyMapping.ContainsKey(column.Trim()))
                            continue;

                        var mapProperty = entityNamePropertyMapping[column.Trim()];

                        var property = newRow.GetType().GetProperty(mapProperty.Name);

                        if (property.PropertyType == typeof(DateTime))
                        {
                            property.SetValue(newRow, TypeParser.Parse<DateTime>(columnValue.ToString()), null);
                        }
                        else if (property.PropertyType == typeof(double))
                        {
                            property.SetValue(newRow, TypeParser.Parse<double>(columnValue.ToString()), null);
                        }
                        else if (property.PropertyType == typeof(decimal))
                        {
                            columnValue = String.IsNullOrEmpty(columnValue.ToString())
                                ? "0"
                                : columnValue.ToString().Replace("\"", ""); //Some files may contain empty cells

                            property.SetValue(newRow, TypeParser.Parse<decimal>(columnValue.ToString()), null);
                        }
                        else if (property.PropertyType == typeof(int))
                        {
                            property.SetValue(newRow, TypeParser.Parse<int>(columnValue.ToString()), null);
                        }
                        else
                        {
                            property.SetValue(newRow, columnValue, null);
                        }

                        columnAdded = true;
                    }
                    catch (KeyNotFoundException ex)
                    {
                        //  throw new FileSchemaMismatchException(
                        //      string.Format("Cannot find a schema property matching column {0} from File", column));
                    }
                    catch (FormatException ex)
                    {
                        throw new FormatException(string.Format(
                            "Parsing Error, Column [ {0} ] value [ {1} ] is Invalid", column, columnValue));
                    }
                }

                if (columnAdded) results.Add(newRow);
            }

            return results;
        }

        private void UpdateColumns(DataTable dataTable)
        {
            Columns.Clear();
            foreach (DataColumn column in dataTable.Columns)
            {
                Columns.Add(column.ColumnName);
            }
        }



        /// <summary>
        /// Renames the columns to that specified in the file schema if the file does not include column headers
        /// </summary>
        /// <param name="data"></param>
        private void UpdateDataTableColumns(DataTable data)
        {
            if (HasHeaderRows)
                return;

            var columns = GetSchemaColumns();

            if (columns.Count() > data.Columns.Count)
                return;

            var i = 0;
            foreach (var column in columns)
            {
                data.Columns[i].ColumnName = column;
                i++;
            }
        }

        /// <summary>
        /// The the expected columns for the calling importer
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetSchemaColumns()
        {
            var type = typeof(T);
            var aliases = new List<string>();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var aliasName =
                    property.GetCustomAttributes(false)
                        .FirstOrDefault(x => x.GetType() == typeof(MappingNameAttribute));

                var names = aliasName == null
                    ? new List<string> { property.Name.Trim() }
                    : ((MappingNameAttribute)aliasName).Names.Select(t => t.Trim());

                //var aliasName = property.GetCustomAttribute(typeof(MappingNameAttribute));
                aliases.AddRange(names);
                if (names.Count() > 1)
                    _multipleAliasColumns.Add(names.ToList());


                var optionlProperty = property.GetCustomAttributes(false)
                    .FirstOrDefault(x => x.GetType() == typeof(OptionalAttribute));

                if (optionlProperty != null)
                {
                    names.ToList().ForEach(x => _optionalColumns.Add(x));
                }
            }

            return aliases;
        }


        protected virtual void OnNotification(string message)
        {
            if (Notification == null)
                return;

            Notification(this, message);
        }

        protected virtual void OnDataExtraction(IEnumerable<T> records)
        {
            if (DataExtraction == null)
                return;

            DataExtraction(this, records);
        }
    }


    internal class Utils
    {
        private static IEnumerable<string> GetEnumNames<T>()
        {
            return Enum.GetNames(typeof(T));
        }

        public static T GetEnumValue<T>(string fieldName) where T : IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var names = GetEnumNames<T>().ToList().ConvertAll(x => x.ToLower());

            var index = names.IndexOf(fieldName.ToLower());

            return (T)Enum.GetValues(typeof(T)).GetValue(index);
        }

        public static DataTable ParseExcelToDataTable(FileInfo file, bool hasHeaders = true, int startAtRow = 1)
        {
            //  if (file.Extension.ToLower() != ".xlsx") return null; // *.xlsx files only

            DataTable output = null;

            if (file.Extension.ToLower().Equals(".xlsx"))
                output = ExcelReaderAlt(file, hasHeaders, startAtRow);

            // DataSet - The result of each spreadsheet will be created in the result.Tables

            var doHeaderChange = false;
            if (output == null)
            {
                /*
                var stream = File.Open(file.FullName, FileMode.Open, FileAccess.Read);

                IExcelDataReader excelReader = file.Extension.ToLower() == ".xlsx"
                    ? ExcelReaderFactory.CreateOpenXmlReader(stream)
                    : ExcelReaderFactory.CreateBinaryReader(stream);
                
                var result = excelReader.AsDataSet();

                if (result == null)
                {
                    throw new Exception(excelReader.ExceptionMessage);
                }

                output = result.Tables[0];
                excelReader.Close();
                doHeaderChange = true;
                */

                output = ReadExcelFile(file);

            }

            // Free resources (IExcelDataReader is IDisposable)

            if (!doHeaderChange) return output;

            // Take the header row and set as column names

            var firstDow = output.Rows[0];

            for (var i = 0; i < output.Columns.Count; i++)
            {
                if (String.IsNullOrEmpty(firstDow[i].ToString().Trim())) continue;
                output.Columns[i].ColumnName = firstDow[i].ToString();
            }

            firstDow.Delete();
            output.AcceptChanges();

            return output;
        }

        private static DataTable ExcelReaderAlt(FileInfo file, bool hasHeaders, int startAtRow)
        {
            ExcelPackage package;

            switch (file.Extension.ToLower())
            {
                case ".xlsx":
                    var stream = File.Open(file.FullName, FileMode.Open, FileAccess.Read);
                    package = new ExcelPackage(stream);
                    stream.Close();
                    break;
                default:
                    // EPPlus only supports *.XLSX
                    throw new Exception(string.Format("Invalid file extension. {0} is not supported.", file.Extension.ToLower()));
            }

            using (package)
            {
                var worksheet = package.Workbook.Worksheets[1];
                var tbl = new DataTable();
                bool hasHeader = hasHeaders;

                foreach (var firstRowCell in worksheet.Cells[startAtRow, 1, 1, worksheet.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }

                var startRow = hasHeader ? startAtRow + 1 : startAtRow;

                for (var rowNum = startRow; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                {
                    var wsRow = worksheet.Cells[rowNum, 1, rowNum, worksheet.Dimension.End.Column];
                    var row = tbl.NewRow();
                    foreach (var cell in wsRow)
                    {
                        if (cell.Start.Column - 1 >= tbl.Columns.Count)
                            continue;

                        row[cell.Start.Column - 1] = cell.Value;
                    }
                    tbl.Rows.Add(row);
                }

                return tbl;
            }
        }



        private static string GetConnectionString(FileInfo file)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();

            //// XLSX - Excel 2007, 2010, 2012, 2013
            //props["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
            //props["Extended Properties"] = "Excel 12.0 XML";
            //props["Data Source"] = "C:\\MyExcel.xlsx";

            // XLS - Excel 2003 and Older
            props["Provider"] = "Microsoft.Jet.OLEDB.4.0";
            props["Extended Properties"] = "Excel 8.0";
            props["Data Source"] = String.Format("{0}", file.FullName);

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            return sb.ToString();
        }

        private static DataTable ReadExcelFile(FileInfo file)
        {
            DataSet ds = new DataSet();

            string connectionString = GetConnectionString(file);

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;

                // Get all Sheets in Excel File
                DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                // Loop through all Sheets to get data
                foreach (DataRow dr in dtSheet.Rows)
                {
                    string sheetName = dr["TABLE_NAME"].ToString();

                    if (!sheetName.EndsWith("$"))
                        continue;

                    // Get all rows from the Sheet
                    cmd.CommandText = "SELECT * FROM [" + sheetName + "]";

                    DataTable dt = new DataTable();
                    dt.TableName = sheetName;

                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    da.Fill(dt);

                    ds.Tables.Add(dt);
                }

                cmd = null;
                conn.Close();
            }

            if (ds.Tables.Count == 0)
                return null;
            return ds.Tables[0];
        }
    }

    internal static class TypeParser
    {

        public static object Parse<T>(string value) where T : IConvertible
        {
            if (typeof(T) == typeof(Enum))
            {
                return Utils.GetEnumValue<T>(value);
            }

            if (typeof(T) == typeof(DateTime))
            {
                return DateTime.Parse(value);
            }

            if (typeof(T) == typeof(int))
            {
                return Convert.ToInt32(value);
            }

            if (typeof(T) == typeof(double))
            {
                return Convert.ToDouble(value);
            }

            if (typeof(T) == typeof(long))
            {
                return Convert.ToInt64(value);
            }

            if (typeof(T) == typeof(decimal))
            {
                return Convert.ToDecimal(value);
            }

            throw new NotImplementedException();
        }

        private static object ParseEnum<T>(string value) where T : IConvertible
        {
            return Utils.GetEnumValue<T>(value);
        }

        private static object ParseDateTime(string value)
        {
            return DateTime.Parse(value);
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MappingNameAttribute : Attribute
    {
        public List<string> Names { get; set; }
        public MappingNameAttribute(params string[] alias)
        {
            Names = new List<string>();
            Names.AddRange(alias);
        }
    }
}
