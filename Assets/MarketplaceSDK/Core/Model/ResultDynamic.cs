using System;
using System.Collections.Generic;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class ResultDynamic
    {
        public List<DataDynamic> Data { get; set; }
        public string NextCursor { get; set; }
        public bool HasNextPage { get; set; }
    }
}
