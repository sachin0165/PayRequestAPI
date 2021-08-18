using Newtonsoft.Json;
using PayRequestWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayRequestWeb.Models
{
    public class ChangePaymentRequestStatus
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("requestGuid")]
        public Guid RequestGuid { get; set; }

        [JsonProperty("paymentStatus")]
        public PaymentStatus PaymentStatus { get; set; }

        [JsonProperty("cancellationTimeUtc")]
        public DateTime? CancellationTimeUtc { get; set; }

        [JsonProperty("paymentTimeUtc")]
        public DateTime? PaymentTimeUtc { get; set; }
    }
}
