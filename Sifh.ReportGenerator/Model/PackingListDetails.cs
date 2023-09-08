using Sifh.ReportGenerator.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sifh.ReportGenerator.Model
{
    [Table("PackingListDetails")]
    public partial class PackingListDetails : IPackingListDetails
    {

        public int PackingListDetailsID { get; set; }
        public int PackingListID { get; set; }
        public int ReceivingNoteItemID { get; set; }
        public int BoxNumber { get; set; }
    }
}
