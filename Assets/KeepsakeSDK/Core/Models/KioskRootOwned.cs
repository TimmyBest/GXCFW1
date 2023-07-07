using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class KioskRootOwned : RootOwned
    {
        public KioskResultOwned Result { get; set; }
    }
}
