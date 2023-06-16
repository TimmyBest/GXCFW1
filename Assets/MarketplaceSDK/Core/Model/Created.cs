using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class Created
    {
        public Owner Owner { get; set; }
        public Reference Reference { get; set; }
    }
}
