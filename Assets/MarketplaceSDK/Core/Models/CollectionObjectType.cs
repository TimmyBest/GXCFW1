using Newtonsoft.Json;
using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class CollectionObjectType : Collection
    {
        public bool Verified { get; set; }
        [JsonProperty("full_type")]
        public string FullType { get; set; }
        public string ObAllowlist { get; set; }
        [JsonProperty("creator")]
        public object Creator { get; set; }
    }
}