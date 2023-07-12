using System;

namespace Sifh.ReportGenerator.Contracts
{
    public interface IReceivingNote
    {
        int ReceivingNoteID { get; set; }
        DateTime? InvoiceDate { get; set; }
        string ReferenceNumber { get; set; }
        DateTime? OrderDate { get; set; }
        int VesselID { get; set; }
        int StatusClassID { get; set; }
        decimal TotalPayments { get; set; }
        string CheckNumber1 { get; set; }
        string CheckNumber2 { get; set; }
        DateTime DateCreated { get; set; }
    }



}