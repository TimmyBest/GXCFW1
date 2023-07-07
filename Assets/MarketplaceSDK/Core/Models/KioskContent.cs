using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class KioskContent : Content
    {
        public KioskFields fields { get; set; }
    }
}