using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sifh.ReportGenerator.Contracts
{
    public interface ICustomer
    {
        int CustomerID { get; set; }
        string CustomerName { get; set; }
    }
}
