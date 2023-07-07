using Newtonsoft.Json;
using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class DataError
    {
        [JsonProperty("details")]
        public string Details { get; set; }
    }
}
