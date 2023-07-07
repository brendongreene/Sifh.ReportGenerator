using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sifh.ReportGenerator.Model
{
    [Table("PackingList")]
    public partial class PackingList
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PackingList()
        {
            ReceivingNoteItems = new HashSet<ReceivingNoteItem>();
        }

        public int PackingListID { get; set; }

        public int CustomerID { get; set; }

        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        public DateTime? DateCreated { get; set; }

        public int StatusClassID { get; set; }

        public int AirlineID { get; set; }

        public virtual Airline Airline { get; set; }

        public virtual Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceivingNoteItem> ReceivingNoteItems { get; set; }
    }
}
