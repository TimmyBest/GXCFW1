using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class NameDynamic
    {
        public string Type { get; set; }
        public Value Value { get; set; }
    }
}