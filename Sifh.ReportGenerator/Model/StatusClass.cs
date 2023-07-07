using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sifh.ReportGenerator.Model
{
    [Table("StatusClass")]
    public partial class StatusClass
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StatusClass()
        {
            ReceivingNotes = new HashSet<ReceivingNote>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StatusClassID { get; set; }

        [Required]
        [StringLength(50)]
        public string StatusClassName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceivingNote> ReceivingNotes { get; set; }
    }
}
