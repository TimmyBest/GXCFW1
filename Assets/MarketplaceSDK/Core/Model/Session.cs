using Newtonsoft.Json;
using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class Session : RootOwned
    {
        [JsonProperty("result")]
        public string Result { get; set; }
    }
}
