using Newtonsoft.Json;

namespace PayRequestWeb.Models
{
    public class SuccessResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
