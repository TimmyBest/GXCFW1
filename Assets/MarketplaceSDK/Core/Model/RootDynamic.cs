using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class RootDynamic : RootOwned
    {
        public ResultDynamic Result { get; set; }
    }
}
