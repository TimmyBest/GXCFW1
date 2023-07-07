using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class KioskRootOwned : RootOwned
    {
        public KioskResultOwned Result { get; set; }
    }
}
