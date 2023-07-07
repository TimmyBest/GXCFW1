using Newtonsoft.Json;
using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class AttributesMulti
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("fields")]
        public FieldsMulti Fields { get; set; }
    }
}