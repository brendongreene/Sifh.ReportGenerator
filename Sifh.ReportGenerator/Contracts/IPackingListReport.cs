using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sifh.ReportGenerator.Contracts
{
    public interface IPackingList
    {
        int PackingListID { get; set; }
        int CustomerID { get; set; }
        string InvoiceNumber { get; set; }
        DateTime? DateCreated { get; set; }
        int StatusClassID { get; set; }
        int AirlineID { get; set; }
        int? BoxNumber { get; set; }
        decimal? Weight { get; set; }
        string BoatName { get; set; }
        int? ReceivingNoteItemID { get; set; }
        int PackingListNumber { get; set; }
    }
}
