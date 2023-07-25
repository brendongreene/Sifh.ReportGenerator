using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;



namespace Sifh.ReportGenerator.Core
{
    internal class ExelToPDF
    {
        public void ConvertExcelToPdfUsingEPPlus(string excelFilePath, string pdfFilePath)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                using (FileStream pdfStream = new FileStream(pdfFilePath, FileMode.Create))
                {
                    Document pdfDocument = new Document(PageSize.A4);
                    PdfWriter.GetInstance(pdfDocument, pdfStream);
                    pdfDocument.Open();

                    for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                    {
                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            string cellValue = worksheet.Cells[row, col].Value?.ToString() ?? "";
                            pdfDocument.Add(new Paragraph(cellValue));
                        }
                    }

                    pdfDocument.Close();
                }
            }
        }
    }
}
