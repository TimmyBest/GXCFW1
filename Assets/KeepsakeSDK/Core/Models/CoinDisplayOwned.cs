using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class CoinDisplayOwned : DisplayOwned
    {
        public CoinDataOwned Data { get; set; }
    }
}
