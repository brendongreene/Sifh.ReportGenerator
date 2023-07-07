using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sifh.ReportGenerator.Contracts;
using Sifh.ReportGenerator.Model;

namespace Sifh.ReportGenerator.DTO
{
    public class ReceivingNoteView: IReceivingNote
    {

        public ReceivingNoteView()
        {

        }

        public ReceivingNoteView(ReceivingNote receivingNote)
        {
            this.ReceivingNoteID = receivingNote.ReceivingNoteID;
            this.InvoiceDate = receivingNote.InvoiceDate;
            this.VesselID = receivingNote.VesselID;
            this.DateCreated = receivingNote.DateCreated;
            this.ReferenceNumber = receivingNote.ReferenceNumber;
            this.VesselName = receivingNote.Vessel.VesselName;
            this.Quantity = receivingNote.ReceivingNoteItems.Sum(x => x.Quantity);
            this.LineItems = receivingNote.ReceivingNoteItems.Count();
            this.RegistrationNumber = receivingNote.Vessel.RegistrationNumber;


        }

        public int ReceivingNoteID { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public int VesselID { get; set; }
        public int StatusClassID { get; set; }
        public decimal TotalPayments { get; set; }
        public string CheckNumber1 { get; set; }
        public string CheckNumber2 { get; set; }
        public DateTime DateCreated { get; set; }
        public string VesselName { get; set; }
        public string RegistrationNumber { get; set; }

        public decimal Quantity { get; set; }
        public int LineItems { get; set; }
    }
}
