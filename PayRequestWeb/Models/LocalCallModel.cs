using Newtonsoft.Json;
using System.Collections.Generic;

namespace PayRequestWeb.Models
{
    public class LocalCallModel
    {
        public LocalCallModel()
        {
            Parameters = new List<string>();
        }

        [JsonProperty("contractAddress")]
        public string ContractAddress { get; set; }

        [JsonProperty("methodName")]
        public string MethodName { get; set; }

        [JsonProperty("amount")]
        public ulong Amount { get; set; }

        [JsonProperty("gasPrice")]
        public int GasPrice { get; set; }

        [JsonProperty("gasLimit")]
        public int GasLimit { get; set; }

        [JsonProperty("sender")]
        public string Sender { get; set; }

        [JsonProperty("parameters")]
        public List<string> Parameters { get; set; }
    }
}
