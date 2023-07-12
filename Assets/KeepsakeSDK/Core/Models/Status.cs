using Newtonsoft.Json;
using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class Status
    {
        [JsonProperty("status")]
        public string status { get; set; }
    }
}
