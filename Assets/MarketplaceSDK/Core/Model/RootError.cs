using Newtonsoft.Json;
using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class RootError : RootOwned
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }
}
