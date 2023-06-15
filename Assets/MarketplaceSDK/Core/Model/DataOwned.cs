using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class DataOwned
    {
        public string ObjectId { get; set; }
        public string Version { get; set; }
        public string Digest { get; set; }
        public string Type { get; set; }
    }
}