using Aspose.Cells;
using Aspose.Cells.Rendering;

namespace Sifh.ReportGenerator.Core
{
    internal class ExcelToPDF
    {
        public void ConvertExcelToPdfUsingAspose(string excelFilePath, string pdfFilePath)
        {
            Workbook workbook = new Workbook(excelFilePath);

            PdfSaveOptions saveOptions = new PdfSaveOptions
            {
                AllColumnsInOnePagePerSheet = true,
                Compliance = PdfCompliance.PdfA1b
            };

            workbook.Save(pdfFilePath, saveOptions);
        }
    }
}
