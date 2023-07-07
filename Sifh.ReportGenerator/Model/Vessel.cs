using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sifh.ReportGenerator.Model
{
    [Table("Vessel")]
    public partial class Vessel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vessel()
        {
            ReceivingNotes = new HashSet<ReceivingNote>();
        }

        public int VesselID { get; set; }

        [Required]
        [StringLength(64)]
        public string VesselName { get; set; }

        [Required]
        [StringLength(256)]
        public string Address { get; set; }

        [Required]
        [StringLength(64)]
        public string ContactName { get; set; }

        [Required]
        [StringLength(50)]
        public string ContactNumber { get; set; }

        [StringLength(50)]
        public string RegistrationNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public bool SoftDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceivingNote> ReceivingNotes { get; set; }
    }
}
