using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class RootBalance : RootOwned
    {
        public ResultBalance Result { get; set; }
    }
}