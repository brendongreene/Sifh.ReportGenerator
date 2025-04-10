using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIFH_ERP_ClassLib.ModelClasses
{
    public class DeliveryNote
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("partner_id")]
        public object PartnerId { get; set; }

        [JsonProperty("route_id")]
        public object RouteId { get; set; }

        [JsonProperty("location_id")]
        public object LocationId { get; set; }

        [JsonProperty("scheduled_date")]
        public string ScheduledDate { get; set; }

        [JsonProperty("date_done")]
        public string DateDone { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("payment_term_id")]
        public object PaymentTermId { get; set; }

        [JsonProperty("carrier_id")]
        public object CarrierId { get; set; }

        [JsonProperty("carrier_tracking_ref")]
        public string CarrierTrackingRef { get; set; }

        [JsonProperty("weight")]
        public string Weight { get; set; }

        [JsonProperty("shipping_weight")]
        public string ShippingWeight { get; set; }

        [JsonProperty("move_type")]
        public string MoveType { get; set; }

        [JsonProperty("user_id")]
        public object UserId { get; set; }

        [JsonProperty("group_id")]
        public object GroupId { get; set; }

        [JsonProperty("airway_no")]
        public object AirwayNo { get; set; }

        [JsonProperty("signature_name")]
        public object SignatureName { get; set; }

        [JsonProperty("fda")]
        public object Fda { get; set; }

        [JsonProperty("ufi")]
        public object Ufi { get; set; }

        [JsonProperty("batch_id")]
        public object BatchId { get; set; }

        [JsonProperty("create_date")]
        public string CreateDate { get; set; }

        [JsonProperty("move_ids")]
        public object MoveIds { get; set; }

        [JsonProperty("lot_id")]
        public object LotNumber { get; set; }
        
        public object VesselName { get; set; }

        public List<DeliveryNoteOperation> DeliveryNoteOperation { get; set; }
    }
    public class groupId
    {
        string Id { get; set; }
        string Name { get; set; }
    }
}
