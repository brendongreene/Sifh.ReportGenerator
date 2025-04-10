using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFH_ERP_ClassLib.ModelClasses
{
    public class Vessel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("sub_contract")]
        public object VesselName { get; set; }
    }
}
