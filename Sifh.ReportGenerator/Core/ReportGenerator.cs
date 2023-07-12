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
            ModelTransshipping,
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

            public void ProcessRecord(ReportDataView record)
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
        private ExcelTemplateInfo modelReprocessingData = new ExcelTemplateInfo();
        private ExcelTemplateInfo modelTransshippingData = new ExcelTemplateInfo();


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
                    CellAddress = "D4"
                },
                new ReportValue()
                {
                    MappingFieldName = "VesselName",
                    CellAddress = "G8"
                },
                new ReportValue()
                {
                    MappingFieldName = "RegistrationNumber",
                    CellAddress = "K8"
                },
                new ReportValue()
                {
                    MappingFieldName = "RegistrationNumber",
                    CellAddress = "B11"
                }
                ,
                new ReportValue()
                {
                    MappingFieldName = "OrderDate",
                    CellAddress = "G25"
                },
                new ReportValue()
                {
                    MappingFieldName = "Quantity",
                    CellAddress = "H29"
                }
            });

            modelReprocessingData.ExcelValues.AddRange(new[]
           {
                new ReportValue()
                {
                    MappingFieldName = "ReceivingNoteID",
                    CellAddress = "E10"
                },
                 new ReportValue()
                {
                    MappingFieldName = "ReceivingNoteID",
                    CellAddress = "J26"
                },
                new ReportValue()
                {
                    MappingFieldName = "VesselName",
                    CellAddress = "I10"
                },
                new ReportValue()
                {
                    MappingFieldName = "ProductName",
                    CellAddress = "I21"
                },

                new ReportValue()
                {
                    MappingFieldName = "CustomerName",
                    CellAddress = "I29"
                },

                new ReportValue()
                {
                    MappingFieldName = "AirwayBillNumber",
                    CellAddress = "I30"
                },
                new ReportValue()
                {
                    MappingFieldName = "ProductionDate",
                    CellAddress = "D34"
                },
                new ReportValue()
                {
                    MappingFieldName = "Quantity",
                    CellAddress = "D26"
                },
                new ReportValue()
                {
                    MappingFieldName = "ReceivingLotIdentifierMRC",
                    CellAddress = "J26"
                },
                new ReportValue()
                {
                    MappingFieldName = "Quantity",
                    CellAddress = "D29"
                },
                new ReportValue()
                {
                    MappingFieldName = "Quantity",
                    CellAddress = "D32"
                },
                new ReportValue()
                {
                    MappingFieldName = "BoxNumber",
                    CellAddress = "L34"
                }

            });
            modelTransshippingData.ExcelValues.AddRange(new[]
          {
                new ReportValue()
                {
                    MappingFieldName = "ReceivingNoteID",
                    CellAddress = "D9"
                },
                new ReportValue()
                {
                    MappingFieldName = "VesselName",
                    CellAddress = "C18"
                },
                new ReportValue()
                {
                    MappingFieldName = "RegistrationNumber",
                    CellAddress = "H18"
                },
                new ReportValue()
                {
                    MappingFieldName = "FormattedDateCreated",
                    CellAddress = "N20"
                },
                new ReportValue()
                {
                    MappingFieldName = "Quantity",
                    CellAddress = "C32"
                },
                new ReportValue()
                {
                    MappingFieldName = "ProductName",
                    CellAddress = "H28"
                },
                new ReportValue()
                {
                    MappingFieldName = "AirwayBillNumber",
                    CellAddress = "H32"
                },
                new ReportValue()
                {
                    MappingFieldName = "BoxNumber",
                    CellAddress = "H35"
                },
                new ReportValue()
                {
                    MappingFieldName = "ProductionDate",
                    CellAddress = "C35"
                }
            });

        }

        public void GenerateExcelReport(ReportType reportType, FileInfo newFile, ReportDataView receivingNoteView)
        {    
            using (var package = new ExcelPackage(newFile, TemplateFile))
            {
                var workbook = package.Workbook;
                modelCatchData.ProcessRecord(receivingNoteView);
                foreach (var excelInfo in modelCatchData.ExcelValues)
                {
                    workbook.Worksheets["MCC"].Cells[excelInfo.CellAddress].Value = excelInfo.Value;
                }
                modelReprocessingData.ProcessRecord(receivingNoteView);
                foreach (var excelInfo in modelReprocessingData.ExcelValues)
                {
                    workbook.Worksheets["MRC"].Cells[excelInfo.CellAddress].Value = excelInfo.Value;
                }
                modelTransshippingData.ProcessRecord(receivingNoteView);
                foreach (var excelInfo in modelTransshippingData.ExcelValues)
                {
                    workbook.Worksheets["MTC"].Cells[excelInfo.CellAddress].Value = excelInfo.Value;
                }
                package.Save();
            }
        }

       
    }
}
