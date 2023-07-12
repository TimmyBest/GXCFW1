using Newtonsoft.Json;
using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class ResultTransaction
    {
        [JsonProperty("digest")]
        public string Digest { get; set; }

        [JsonProperty("effects")]
        public Effects Effects { get; set; }

        [JsonProperty("confirmedLocalExecution")]
        public bool ConfirmedLocalExecution { get; set; }
    }
}
