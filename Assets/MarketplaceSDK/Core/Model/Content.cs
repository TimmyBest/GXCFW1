using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class Content
    {
        public string DataType { get; set; }
        public string Type { get; set; }
        public bool HasPublicTransfer { get; set; }
    }
}