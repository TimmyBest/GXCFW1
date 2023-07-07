using System;
using System.Collections.Generic;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class KioskResultOwned : ResultOwned
    {
        public List<KioskDataOwned> Data { get; set; }
    }
}
