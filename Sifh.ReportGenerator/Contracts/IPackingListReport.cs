using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sifh.ReportGenerator.Contracts
{
    public interface IPackingListReport
    {
        int PackingListId { get; set; }
        int ReceivingNoteID { get; set; }
        int CustomerId { get; set; }
        string InvoiceNumber { get; set; }
        DateTime DateCreated { get; set; }
        int StatusClassId { get; set; }
        int AirlineId { get; set; }
        int? BoxNumber { get; set; }
        decimal? Weight { get; set; }
        string BoatName { get; set; }
        int ReceivingNoteItemID { get; set; }
        int PackingListNumber { get; set; }
    }
}
