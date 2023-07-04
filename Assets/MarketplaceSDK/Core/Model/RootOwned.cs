using Newtonsoft.Json;
using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class RootOwned
    {
        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}