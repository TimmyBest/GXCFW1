using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class DataMulti
    {
        public string ObjectId { get; set; }
        public string Version { get; set; }
        public string Digest { get; set; }
        public string Type { get; set; }
        public Owner Owner { get; set; }
        public string PreviousTransaction { get; set; }
        public string StorageRebate { get; set; }
        public ContentMulti Content { get; set; }
    }
}