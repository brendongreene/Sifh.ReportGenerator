using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sifh.ReportGenerator.Model
{
    [Table("GradeClass")]
    public partial class GradeClass
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GradeClass()
        {
            ReceivingNoteItems = new HashSet<ReceivingNoteItem>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GradeClassID { get; set; }

        [Required]
        [StringLength(50)]
        public string GradeClassName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceivingNoteItem> ReceivingNoteItems { get; set; }
    }
}
