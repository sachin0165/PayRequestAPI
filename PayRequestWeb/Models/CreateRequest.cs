using Newtonsoft.Json;
using PayRequestWeb.Helpers;
using System;

namespace PayRequestWeb.Models
{
    public class CreateRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("requestGuid")]
        public Guid RequestGuid { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("fromAddress")]
        public string FromAddress { get; set; }

        [JsonProperty("toAddress")]
        public string ToAddress { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("amountInSatoshi")]
        public long AmountInSatoshi { get; set; }

        [JsonProperty("paymentStatus")]
        public PaymentStatus PaymentStatus { get; set; }

        [JsonProperty("expiry")]
        public DateTime Expiry { get; set; }

        [JsonProperty("expiryInUnixTimestamp")]
        public long ExpiryInUnixTimestamp { get; set; }

        [JsonProperty("CreationTimeUtc")]
        public DateTime CreationTimeUtc { get; set; }

        [JsonProperty("cancellationTimeUtc")]
        public DateTime? CancellationTimeUtc { get; set; }

        [JsonProperty("paymentTimeUtc")]
        public DateTime? PaymentTimeUtc { get; set; }
    }
}
