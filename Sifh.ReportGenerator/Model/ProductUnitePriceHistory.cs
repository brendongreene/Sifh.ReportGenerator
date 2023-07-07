using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sifh.ReportGenerator.Model
{
    [Table("ProductUnitePriceHistory")]
    public partial class ProductUnitePriceHistory
    {
        [Key]
        public int ProductUnitPriceHistoryID { get; set; }

        public int ProductID { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPriceOld { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPriceNew { get; set; }

        public DateTime DateChanged { get; set; }

        public virtual Product Product { get; set; }
    }
}
