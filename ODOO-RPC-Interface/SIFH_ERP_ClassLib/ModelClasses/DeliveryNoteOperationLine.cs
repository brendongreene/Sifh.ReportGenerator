using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFH_ERP_ClassLib.ModelClasses
{
    public class DeliveryNoteOperationLine
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("picking_id")]
        public object PickingId { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("product_uom_id")]
        public object ProductUomId { get; set; }

        [JsonProperty("product_id")]
        public object ProductId { get; set; }

        [JsonProperty("picking_partner_id")]
        public object PickingPartnerId { get; set; }

        [JsonProperty("quant_id")]
        public object Vessel { get; set; }
    }
}
