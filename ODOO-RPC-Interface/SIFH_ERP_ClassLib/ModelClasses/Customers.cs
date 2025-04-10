using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFH_ERP_ClassLib.ModelClasses
{
    public class Customers
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("commercial_company_name")]
        public string CommercialCompanyName { get; set; }

        [JsonProperty("company_name")]
        public object CompanyName { get; set; }

        [JsonProperty("company_type")]
        public string CompanyType { get; set; }

        [JsonProperty("category_id")]
        public object CategoryId { get; set; }

        [JsonProperty("industry_id")]
        public object IndustryId { get; set; }

        [JsonProperty("contact_address")]
        public string ContactAddress { get; set; }
    }
}
