using System;
using System.Collections.Generic;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class Root
    {
        public List<Result> Results { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
        public int PageLimit { get; set; }
    }
}