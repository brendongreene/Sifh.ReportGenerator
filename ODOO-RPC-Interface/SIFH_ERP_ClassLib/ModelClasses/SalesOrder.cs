using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SIFH_ERP_ClassLib.ModelClasses
{
    public class SaleOrder
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("partner_id")]
        public object PartnerId { get; set; }

        [JsonProperty("partner_invoice_id")]
        public object PartnerInvoiceId { get; set; }

        [JsonProperty("partner_shipping_id")]
        public object PartnerShippingId { get; set; }

        [JsonProperty("so_type")]
        public string SoType { get; set; }

        [JsonProperty("date_order")]
        public string DateOrder { get; set; }

        [JsonProperty("pricelist_id")]
        public object PriceListId { get; set; }

        [JsonProperty("payment_term_id")]
        public object PaymentTermId { get; set; }

        [JsonProperty("client_order_ref")]

        public string ClientOrderRef { get; set; }

        [JsonProperty("warehouse_id")]
        public object WarehouseId { get; set; }

        [JsonProperty("is_yft_export")]
        public string IsYftExport { get; set; }

        public List<SalesOrderLine> Lines { get; set; }
    }
}
