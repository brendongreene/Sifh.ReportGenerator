using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sifh.ReportGenerator.Contracts;
using Sifh.ReportGenerator.Model;
using System.Globalization;

namespace Sifh.ReportGenerator.DTO
{
    public class ConductorView: IConductor
    {
        public ConductorView()
        {
        }

        public int ConductorID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }
        public string LicenseNumber { get; set; }
    }
}
