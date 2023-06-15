using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class KioskDisplayOwned : DisplayOwned
    {
        public KioskDataOwned Data { get; set; }
    }
}
