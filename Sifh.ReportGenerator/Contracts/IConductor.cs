using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sifh.ReportGenerator.Contracts
{
    public interface IConductor
    {
        int ConductorID { get; set; }
        string LastName { get; set; }
        string FirstName { get; set; }
        string Name { get; set; }
        string LicenseNumber { get; set; }
    }
}
