using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class RootDev : RootOwned
    {
        public ResultDev Result { get; set; }
    }
}
