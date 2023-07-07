using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class CoinRootOwned : RootOwned
    {
        public CoinResultOwned Result { get; set; }
    }
}
