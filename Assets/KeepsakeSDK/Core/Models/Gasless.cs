using Newtonsoft.Json;
using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class Gasless
    {
        [JsonProperty("gasless_tx")]
        public string GaslessTx { get; set; }
    }
}
