using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFH_ERP_ClassLib.ModelClasses
{
    public class SalesOrderLine
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("order_id")]
        public object OrderId { get; set; }

        [JsonProperty("product_template_id")]
        public object ProductTemplateId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("location_id")]
        public object LocationId { get; set; }

        [JsonProperty("weight")]

        public string Weight { get; set; }

        [JsonProperty("lot_id")]
        public string LotId { get; set; }

        [JsonProperty("product_uom_qty")]
        public string ProductUomQty { get; set; }

        [JsonProperty("qty_delivered")]
        public string QtyDelivered { get; set; }

        [JsonProperty("qty_invoiced")]
        public string QtyInvoiced { get; set; }

        [JsonProperty("product_uom")]
        public object ProductUom { get; set; }

        [JsonProperty("price_unit")]
        public string PriceUnit { get; set; }

        [JsonProperty("tax_id")]
        public object TaxId { get; set; }

        [JsonProperty("discount")]
        public string Discount { get; set; }

        [JsonProperty("price_subtotal")]
        public string PriceSubtotal { get; set; }
    }
}
