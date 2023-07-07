using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class Field
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<string> Values { get; set; }
        public float Size { get; set; }
        public float Speed { get; set; }

        [JsonProperty("edge color")]
        public string EdgeColor { get; set; }

        [JsonProperty("side color")]
        public string SideColor { get; set; }
    }
}