using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class RootDev : RootOwned
    {
        public ResultDev Result { get; set; }
    }
}
