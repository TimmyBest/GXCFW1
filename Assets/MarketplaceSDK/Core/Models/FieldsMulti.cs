using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class FieldsMulti
    {
        [JsonProperty("attributes")]
        public AttributesMulti Attributes { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public IdOwned Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("map")]
        public MapMulti map { get; set; }

        [JsonProperty("contents")]
        public List<ContentMulti> Contents { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}