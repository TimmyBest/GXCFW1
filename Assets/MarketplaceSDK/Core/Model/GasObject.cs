using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class GasObject
    {
        public Owner Owner { get; set; }
        public Reference Reference { get; set; }
    }
}