using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class ResultOwned
    {
        public string NextCursor { get; set; }
        public bool HasNextPage { get; set; }
    }
}
