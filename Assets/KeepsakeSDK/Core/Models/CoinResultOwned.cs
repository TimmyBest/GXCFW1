using System;
using System.Collections.Generic;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class CoinResultOwned : ResultOwned
    {
        public List<CoinDataOwned> Data { get; set; }
    }
}
