using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MarketplaceSDK.Models
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