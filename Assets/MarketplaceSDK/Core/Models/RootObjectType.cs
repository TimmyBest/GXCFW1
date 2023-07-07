using Newtonsoft.Json;
using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class RootObjectType
    {
        [JsonProperty("collection")]
        public CollectionObjectType Collection { get; set; }
    }
}
