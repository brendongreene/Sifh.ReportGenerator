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
    public class TruckView : ITruck
    {
        public TruckView()
        {
        }

        public int TruckID { get; set; }
        public string License { get; set; }
    }
}
