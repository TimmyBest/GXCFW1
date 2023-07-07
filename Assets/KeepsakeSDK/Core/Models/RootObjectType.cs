using Newtonsoft.Json;
using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class RootObjectType
    {
        [JsonProperty("collection")]
        public CollectionObjectType Collection { get; set; }
    }
}
