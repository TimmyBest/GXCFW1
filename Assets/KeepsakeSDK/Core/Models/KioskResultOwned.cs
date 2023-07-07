using System;
using System.Collections.Generic;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class KioskResultOwned : ResultOwned
    {
        public List<KioskDataOwned> Data { get; set; }
    }
}
