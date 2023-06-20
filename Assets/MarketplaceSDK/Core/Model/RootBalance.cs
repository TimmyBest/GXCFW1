using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class RootBalance : RootOwned
    {
        public ResultBalance Result { get; set; }
    }
}