using Newtonsoft.Json;
using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class DataError
    {
        [JsonProperty("details")]
        public string Details { get; set; }
    }
}
