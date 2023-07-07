using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class ObRoyalty
    {
        public string Type { get; set; }
        public bool SpecifyType { get; set; }
        public int Amount { get; set; }
        public string Id { get; set; }
        public string PackageId { get; set; }
        public string _Id { get; set; }
    }
}