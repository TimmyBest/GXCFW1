using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class CoinRootOwned : RootOwned
    {
        public CoinResultOwned Result { get; set; }
    }
}
