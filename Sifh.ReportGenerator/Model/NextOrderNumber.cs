using System.ComponentModel.DataAnnotations.Schema;

namespace Sifh.ReportGenerator.Model
{
    [Table("NextOrderNumber")]
    public partial class NextOrderNumber
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NextOrderNumberID { get; set; }

        public int OrderNumber { get; set; }
    }
}
