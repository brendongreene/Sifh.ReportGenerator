using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sifh.ReportGenerator.Contracts
{
    public interface ITruck
    {
        int TruckID { get; set; }
        string License { get; set; }
    }
}
