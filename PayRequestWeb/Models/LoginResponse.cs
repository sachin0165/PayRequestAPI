using Newtonsoft.Json;

namespace PayRequestWeb.Models
{
    public class LoginResponse
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("walletAddress")]
        public string WalletAddress { get; set; }
    }
}
