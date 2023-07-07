using Newtonsoft.Json;
using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class ContentMulti
    {
        [JsonProperty("dataType")]
        public string DataType { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("hasPublicTransfer")]
        public bool HasPublicTransfer { get; set; }

        [JsonProperty("fields")]
        public FieldsMulti Fields { get; set; }
    }
}