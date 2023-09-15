using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class Field
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<string> Values { get; set; }
        public float Size { get; set; }
        public float Speed { get; set; }

        //my own custom fields game related
        public float Level { get; set; }
        public float Damage { get; set; }
        public float ManaCost { get; set; }
        public float Health { get; set; }

        [JsonProperty("edge color")]
        public string EdgeColor { get; set; }

        [JsonProperty("side color")]
        public string SideColor { get; set; }
    }
}