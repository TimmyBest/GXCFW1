using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class RootDynamic : RootOwned
    {
        public ResultDynamic Result { get; set; }
    }
}
