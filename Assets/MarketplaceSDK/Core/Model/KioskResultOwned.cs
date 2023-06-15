using System;
using System.Collections.Generic;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class KioskResultOwned : ResultOwned
    {
        public List<KioskDataOwned> Data { get; set; }
    }
}
