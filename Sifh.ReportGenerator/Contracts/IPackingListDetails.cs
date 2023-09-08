using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sifh.ReportGenerator.Contracts
{
    public interface IPackingListDetails
    {
        int PackingListDetailsID { get; set; }
        int PackingListID { get; set; }
        int ReceivingNoteItemID { get; set; }
        int BoxNumber { get; set; }
    }
}
