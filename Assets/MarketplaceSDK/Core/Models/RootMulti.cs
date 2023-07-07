using System;
using System.Collections.Generic;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class RootMulti : RootOwned
    {
        public List<ResultMulti> Result { get; set; }
    }
}
