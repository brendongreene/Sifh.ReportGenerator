using Sifh.ReportGenerator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sifh.ReportGenerator.DTO
{
    public class ReportFromPackingListView : IReceivingNote
    {
        public int ReceivingNoteID { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public int VesselID { get; set; }
        public string VesselName { get; set; }
        public string CustomerName { get; set; }
        public string AirwayBillNumber { get; set; }
        public decimal GrossQuantity { get; set; }
        public string RegistrationNumber { get; set; }
        public decimal Quantity { get; set; }
        public string ProductName { get; set; }
        public int StatusClassID { get; set; }
        public decimal TotalPayments { get; set; }
        public string CheckNumber1 { get; set; }
        public string CheckNumber2 { get; set; }
        public DateTime DateCreated { get; set; }
        public string ProductionDate { get; set; }
        public string ReceivingLotIdentifierMRC { get; set; }
        public int BoxNumber { get; set; }
        public string FormattedDateCreated { get; set; }
    }
}
