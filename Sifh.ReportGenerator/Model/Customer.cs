using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sifh.ReportGenerator.Contracts;

namespace Sifh.ReportGenerator.Model
{
    [Table("Customer")]
    public partial class Customer:ICustomer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            PackingLists = new HashSet<PackingList>();
        }

        public int CustomerID { get; set; }

        [StringLength(50)]
        public string CustomerName { get; set; }

        public bool SoftDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PackingList> PackingLists { get; set; }
    }
}
