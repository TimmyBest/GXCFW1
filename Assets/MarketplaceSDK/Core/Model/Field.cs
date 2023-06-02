using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class Field
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<string> Values { get; set; }
        public int Size { get; set; }
        public int Speed { get; set; }

        [JsonProperty("edge color")]
        public string EdgeColor { get; set; }

        [JsonProperty("side color")]
        public string SideColor { get; set; }
    }
}