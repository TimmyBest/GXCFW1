using Newtonsoft.Json;
using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class RootTransaction : RootOwned
    {
        [JsonProperty("result")]
        public ResultTransaction Result { get; set; }
    }
}
