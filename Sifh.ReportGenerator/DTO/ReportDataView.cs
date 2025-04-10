using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sifh.ReportGenerator.Contracts;
using Sifh.ReportGenerator.Model;
using System.Globalization;

namespace Sifh.ReportGenerator.DTO
{
    public class ReportDataView : IReceivingNote
    {

        public ReportDataView()
        {
            ReceivingNoteDetails = new List<ReceivingNoteItemView>();
        }

        public ReportDataView(ReceivingNote receivingNote)
        {
            
            this.ReceivingNoteID = receivingNote.ReceivingNoteID;
            this.VesselID = receivingNote.VesselID;
            this.DateCreated = receivingNote.DateCreated;
            this.ReferenceNumber = receivingNote.ReferenceNumber;
            this.VesselName = receivingNote.Vessel.VesselName;
            this.Quantity = receivingNote.ReceivingNoteItems.Sum(x => x.Quantity);
            this.NetQuantity = this.Quantity * ConversionToKg;

            this.RegistrationNumber = receivingNote.Vessel.RegistrationNumber;
            this.ProductName = receivingNote.ReceivingNoteItems.FirstOrDefault()?.Product.ProductName;
            this.InvoiceDate = receivingNote.InvoiceDate;
            this.ReceivingLotIdentifierMRC = receivingNote.ReceivingNoteID.ToString() + "/" + receivingNote.ReferenceNumber.ToString();
            this.OrderDate = receivingNote.OrderDate;
            this.TotalPayments = receivingNote.TotalPayments;
            this.StatusClassID = receivingNote.StatusClassID;

            this.ReceivingNoteDetails = receivingNote.ReceivingNoteItems.Where(x => x.PackingListID == null && x.ProductStatusClassID == 10).Select(x => new ReceivingNoteItemView(x)).ToList();
            this.LineItems = this.ReceivingNoteDetails.Count(); // receivingNote.ReceivingNoteItems.Count();
            //this.VesselDocument = receivingNote.VesselCertificate.VesselDocument;
            //this.CustomerDocument = receivingNote.CustomerFiles.CustomerDocument;
        }


        public string FormattedDateCreated
        {
            get
            {
                return DateCreated.ToString("MMMM dd yyyy", CultureInfo.InvariantCulture);
            }
        }

        public decimal ConversionToKg { get; set; } = 0.45m;
        public int ReceivingNoteID { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public int PackingListID { get; set; }
        public int VesselID { get; set; }
        public int StatusClassID { get; set; }
        public decimal TotalPayments { get; set; }
        public string CheckNumber1 { get; set; }
        public string CheckNumber2 { get; set; }
        public DateTime DateCreated { get; set; }
        public string VesselName { get; set; }
        public string RegistrationNumber { get; set; }

        public decimal Quantity { get; set; }
        public decimal NetQuantity { get; set; }
        public int LineItems { get; set; }
        public string ProductName { get; set; }

        public string CustomerName { get; set; }
        public string CustomerID { get; set; }
        public string AirwayBillNumber { get; set; }
        public string ProductionDate { get; set; }
        public string ReceivingLotIdentifierMRC { get; set; }
        public int BoxNumber { get; set; }
        public int TotalBoxes { get; set; }

        public string ConductorName { get; set; }
        public string ConductorLicense { get; set; }

        public string TruckLicense { get; set; }

        //public byte[] VesselDocument { get; set; }
        //public byte[] CustomerDocument { get; set; }
        public int VesselIDForLicence { get; set; }

        public string InvoiceNumber { get; set; }


        public List<ReceivingNoteItemView> ReceivingNoteDetails { get; set; }
    }
}
