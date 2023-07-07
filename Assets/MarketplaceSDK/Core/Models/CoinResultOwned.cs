using System;
using System.Collections.Generic;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class CoinResultOwned : ResultOwned
    {
        public List<CoinDataOwned> Data { get; set; }
    }
}
