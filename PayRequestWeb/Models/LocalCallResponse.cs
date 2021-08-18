using Newtonsoft.Json;
using System.Collections.Generic;

namespace PayRequestWeb.Models
{
    public class LocalCallResponse
    {
        [JsonProperty("internalTransfers")]
        public List<object> InternalTransfers { get; set; }

        [JsonProperty("gasConsumed")]
        public GasConsumed GasConsumed { get; set; }

        [JsonProperty("revert")]
        public bool Revert { get; set; }

        [JsonProperty("errorMessage")]
        public object ErrorMessage { get; set; }

        [JsonProperty("return")]
        public long Return { get; set; }

        [JsonProperty("logs")]
        public List<object> Logs { get; set; }
    }

    public class GasConsumed
    {
        [JsonProperty("value")]
        public int Value { get; set; }
    }
}
