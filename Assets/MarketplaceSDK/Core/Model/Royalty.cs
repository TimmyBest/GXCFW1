using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class Royalty
    {
        public string Type { get; set; }
        public int Amount { get; set; }
        public string Id { get; set; }
        public string PackageId { get; set; }
        public string _Id { get; set; }
    }
}