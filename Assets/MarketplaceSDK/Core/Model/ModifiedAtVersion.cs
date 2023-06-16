using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class ModifiedAtVersion
    {
        public string ObjectId { get; set; }
        public string SequenceNumber { get; set; }
    }
}