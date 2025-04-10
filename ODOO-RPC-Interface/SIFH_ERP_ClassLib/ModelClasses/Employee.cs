using Newtonsoft.Json;

namespace SIFH_ERP_ClassLib.ModelClasses
{
    public class Employee
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public string DriversLicenseNumber { get; set; }
    }
}
