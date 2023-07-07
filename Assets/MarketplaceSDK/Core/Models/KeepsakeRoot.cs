using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class KeepsakeRoot
    {
        public string status { get; set; }
        public string token { get; set; }
        public Data data { get; set; }
    }
}
