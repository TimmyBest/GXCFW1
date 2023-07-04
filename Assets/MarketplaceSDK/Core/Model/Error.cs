using Newtonsoft.Json;
using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class Error
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public DataError Data { get; set; }
    }
}
