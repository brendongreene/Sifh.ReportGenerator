using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using Sifh.ReportGenerator.DTO;

namespace Sifh.ReportGenerator.Core
{
    public class ReportGenerator
    {
        public enum ReportType
        {
            ModelCatch,
            ModelReprocessing,
            All
        }

        public class ReportValue
        {
            public string MappingFieldName { get; set; }
            public string CellAddress { get; set; }
            public object Value { get; set; }
        }

        public class ExcelTemplateInfo
        {
            public ExcelTemplateInfo()
            {
                ExcelValues = new List<ReportValue>();
            }

            public ReportType ReportType { get; set; }
            public List<ReportValue> ExcelValues { get; set; }

            public void ProcessRecord(ReceivingNoteView record)
            {
                foreach (var dataNum in ExcelValues)
                {
                    dataNum.Value = record.GetType().GetProperty(dataNum.MappingFieldName).GetValue(record);
                }
            }
        }


        public FileInfo TemplateFile { get; set; }
        private List<ExcelTemplateInfo> excelTemplateData = new List<ExcelTemplateInfo>();
        private ExcelTemplateInfo modelCatchData = new ExcelTemplateInfo();


        public ReportGenerator()
        {
            TemplateFile = new FileInfo(@"Excel Templates\Templates.xlsx");
            Setup();
        }

        private void Setup()
        {


            modelCatchData.ExcelValues.AddRange(new[]
            {
                new ReportValue()
                {
                    MappingFieldName = "ReceivingNoteID",
                    CellAddress = "D7"
                },
                new ReportValue()
                {
                    MappingFieldName = "VesselName",
                    CellAddress = "H11"
                },
                new ReportValue()
                {
                    MappingFieldName = "RegistrationNumber",
                    CellAddress = "C14"
                },
                new ReportValue()
                {
                    MappingFieldName = "RegistrationNumber",
                    CellAddress = "M11"
                }
                ,
                new ReportValue()
                {
                    MappingFieldName = "OrderDate",
                    CellAddress = "I28"
                },
                new ReportValue()
                {
                    MappingFieldName = "Quantity",
                    CellAddress = "I32"
                }
            });

        }

        public void GenerateExcelReport(ReportType reportType,FileInfo newFile,ReceivingNoteView receivingNoteView)
        {
            modelCatchData.ProcessRecord(receivingNoteView);
            using (var package = new ExcelPackage(newFile, TemplateFile))
            {
                foreach (var excelInfo in modelCatchData.ExcelValues)
                {
                    package.Workbook.Worksheets["MCC"].Cells[excelInfo.CellAddress].Value = excelInfo.Value;
                }

                package.Save();
            }
        }
    }
}
