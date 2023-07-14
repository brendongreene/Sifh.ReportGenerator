using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sifh.ReportGenerator.Contracts;
using Sifh.ReportGenerator.Model;

namespace Sifh.ReportGenerator.DTO
{
    internal class VesselView : IVesselView
    {

        public VesselView(Vessel vessel) {
            this.VesselID = vessel.VesselID;
            this.VesselName = vessel.VesselName;
        }

        public int VesselID { get; set; }
        public string VesselName { get; set; }
    }
}
