using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class CoinDataOwned : DataOwned
    {
        public CoinDisplayOwned Display { get; set; }
        public CoinContent Content { get; set; }
        public CoinDataOwned Data { get; set; }
    }
}