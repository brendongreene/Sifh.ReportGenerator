using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sifh.ReportGenerator.Model
{
    [Table("ProductStatusClass")]
    public partial class ProductStatusClass
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductStatusClass()
        {
            ReceivingNoteItems = new HashSet<ReceivingNoteItem>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductStatusClassID { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductStatusClassName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceivingNoteItem> ReceivingNoteItems { get; set; }
    }
}
