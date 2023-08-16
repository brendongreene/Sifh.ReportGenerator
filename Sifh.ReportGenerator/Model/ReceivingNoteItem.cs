using Sifh.ReportGenerator.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sifh.ReportGenerator.Model
{
    [Table("ReceivingNoteItem")]
    public partial class ReceivingNoteItem : IReceivingNoteItem
    {
        public int ReceivingNoteItemID { get; set; }

        public int ReceivingNoteID { get; set; }

        public int ProductID { get; set; }

        public decimal Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? LineTotal { get; set; }

        public decimal? Temperature { get; set; }

        public int GradeClassID { get; set; }

        public int ProductStatusClassID { get; set; }

        public int? PackingListID { get; set; }

        [StringLength(16)]
        public string SpeciesCode { get; set; }

        public int? PackingListNumber { get; set; }

        public virtual GradeClass GradeClass { get; set; }

        public virtual PackingList PackingList { get; set; }

        public virtual Product Product { get; set; }

        public virtual ProductStatusClass ProductStatusClass { get; set; }

        public virtual ReceivingNote ReceivingNote { get; set; }
    }
}
