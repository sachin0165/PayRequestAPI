using Newtonsoft.Json;

namespace PayRequestWeb.Models
{
    public class BalanceResponse
    {
        [JsonProperty("balance")]
        public decimal Balance { get; set; }
    }
}
