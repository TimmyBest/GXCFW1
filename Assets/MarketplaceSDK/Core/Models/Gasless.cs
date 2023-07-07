using Newtonsoft.Json;
using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class Gasless
    {
        [JsonProperty("gasless_tx")]
        public string GaslessTx { get; set; }
    }
}
