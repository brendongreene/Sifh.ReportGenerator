using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sifh.ReportGenerator.Contracts;

namespace Sifh.ReportGenerator.DTO
{
    public class CustomerView : ICustomer
    {
      public  int CustomerID { get; set; }
      public  string CustomerName { get; set; }
    }
}
