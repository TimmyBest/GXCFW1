using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class Package
    {
        [JsonProperty("object_ids")]
        public PackageType ObjectIds { get; set; }
    }
}