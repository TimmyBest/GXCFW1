using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class KioskContent : Content
    {
        public KioskFields fields { get; set; }
    }
}