using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class FieldsGetObject
    {
        [JsonProperty("profits")]
        public string Profits { get; set; }
    }
}