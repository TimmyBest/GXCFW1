using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class KioskRootOwned : RootOwned
    {
        public KioskResultOwned Result { get; set; }
    }
}
