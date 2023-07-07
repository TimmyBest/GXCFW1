using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class Reference
    {
        public string ObjectId { get; set; }
        public object Version { get; set; }
        public string Digest { get; set; }
    }
}