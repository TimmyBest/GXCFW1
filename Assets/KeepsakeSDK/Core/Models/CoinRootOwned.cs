using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class CoinRootOwned : RootOwned
    {
        public CoinResultOwned Result { get; set; }
    }
}
