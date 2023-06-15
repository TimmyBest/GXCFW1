using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class CoinDisplayOwned : DisplayOwned
    {
        public CoinDataOwned Data { get; set; }
    }
}
