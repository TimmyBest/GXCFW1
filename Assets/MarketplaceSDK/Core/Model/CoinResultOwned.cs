using System;
using System.Collections.Generic;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class CoinResultOwned : ResultOwned
    {
        public List<CoinDataOwned> Data { get; set; }
    }
}
