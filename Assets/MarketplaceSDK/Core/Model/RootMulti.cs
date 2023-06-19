using System;
using System.Collections.Generic;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class RootMulti : RootOwned
    {
        public List<ResultMulti> Result { get; set; }
    }
}
