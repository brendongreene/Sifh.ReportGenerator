using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFH_ERP_ClassLib.ModelClasses
{
    public class DeliveryNoteOperation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("product_id")]
        public object ProductId { get; set; }

        [JsonProperty("group_id")]
        public object GroupId { get; set; }

        [JsonProperty("product_uom_qty")]
        public string ProductUomQty { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("end_quantity")]
        public string EndQuantity { get; set; }

        [JsonProperty("product_uom")]
        public object ProductUom { get; set; }

        [JsonProperty("picking_id")]
        public object PickingId { get; set; }


        [JsonProperty("move_line_ids")]
        public object MoveLineIds { get; set; }

        [JsonProperty("box_no_int")]
        public int BoxNumber { get; set; }

        public List<DeliveryNoteOperationLine> DeliveryNoteOperationLine { get; set; }
    }
}
