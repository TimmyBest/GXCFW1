using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class KioskDataOwned : DataOwned
    {
        public string Description { get; set; }
        public string Kiosk { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public KioskDisplayOwned Display { get; set; }
        public KioskContent Content { get; set; }
        public KioskDataOwned Data { get; set; }
    }
}