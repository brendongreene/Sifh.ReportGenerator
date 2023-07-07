using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sifh.ReportGenerator.Contracts;

namespace Sifh.ReportGenerator.Model
{
    [Table("ReceivingNote")]
    public partial class ReceivingNote : IReceivingNote
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReceivingNote()
        {
            ReceivingNoteItems = new HashSet<ReceivingNoteItem>();
        }

        public int ReceivingNoteID { get; set; }

        public DateTime? InvoiceDate { get; set; }

        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        public DateTime? OrderDate { get; set; }

        public int VesselID { get; set; }

        public int StatusClassID { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalPayments { get; set; }

        [StringLength(50)]
        public string CheckNumber1 { get; set; }

        [StringLength(50)]
        public string CheckNumber2 { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual StatusClass StatusClass { get; set; }

        public virtual Vessel Vessel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceivingNoteItem> ReceivingNoteItems { get; set; }
    }
}
