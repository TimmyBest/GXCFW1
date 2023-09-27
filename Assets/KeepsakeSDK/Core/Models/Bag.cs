using Newtonsoft.Json;
using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class Bag{
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("kiosk_id")]
        public string KioskId { get; set; }
    }
}