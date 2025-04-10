namespace Sifh.ReportGenerator.Contracts
{
    public interface IReceivingNoteItem
    {

        int GradeClassID { get; set; }
        decimal? LineTotal { get; set; }
        int? PackingListID { get; set; }
        int ProductID { get; set; }
        int ProductStatusClassID { get; set; }
        decimal Quantity { get; set; }
        int ReceivingNoteID { get; set; }
        int ReceivingNoteItemID { get; set; }
        string SpeciesCode { get; set; }
        decimal? Temperature { get; set; }
        decimal UnitPrice { get; set; }
        string ProductName { get; set; }
    }
}