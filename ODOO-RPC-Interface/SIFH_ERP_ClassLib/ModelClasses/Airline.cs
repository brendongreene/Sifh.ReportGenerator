using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFH_ERP_ClassLib.Models
{
    public class Airline
    {
        public string AirlineId { get; set; }
        public string AirlineName { get; set; }
        public List<object> PackingList { get; set; }
    }
}
