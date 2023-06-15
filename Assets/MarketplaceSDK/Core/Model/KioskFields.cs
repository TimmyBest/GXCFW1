using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class KioskFields : Fields
    {
        public string Kiosk { get; set; }
        public string Owner { get; set; }
    }
}
