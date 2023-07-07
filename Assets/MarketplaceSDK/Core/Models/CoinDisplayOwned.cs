using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class CoinDisplayOwned : DisplayOwned
    {
        public CoinDataOwned Data { get; set; }
    }
}
