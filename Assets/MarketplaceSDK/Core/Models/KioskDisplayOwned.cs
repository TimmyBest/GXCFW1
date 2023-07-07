using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class KioskDisplayOwned : DisplayOwned
    {
        public KioskDataOwned Data { get; set; }
    }
}
