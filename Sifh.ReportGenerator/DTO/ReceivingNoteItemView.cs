using Sifh.ReportGenerator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sifh.ReportGenerator.DTO
{
    public class ReceivingNoteItemView : IReceivingNoteItem
    {

        public ReceivingNoteItemView(IReceivingNoteItem receivingNoteItem)
        {
            this.LineTotal = receivingNoteItem.LineTotal;
            this.Quantity = receivingNoteItem.Quantity;
            this.SpeciesCode = receivingNoteItem.SpeciesCode;
        }

        public int GradeClassID { get; set; }
        public decimal? LineTotal { get; set; }
        public int? PackingListID { get; set; }
        public int ProductID { get; set; }
        public int ProductStatusClassID { get; set; }
        public decimal Quantity { get; set; }
        public int ReceivingNoteID { get; set; }
        public int ReceivingNoteItemID { get; set; }
        public string SpeciesCode { get; set; }
        public decimal? Temperature { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
