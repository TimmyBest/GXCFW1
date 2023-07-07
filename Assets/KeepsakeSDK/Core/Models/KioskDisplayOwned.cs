using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class KioskDisplayOwned : DisplayOwned
    {
        public KioskDataOwned Data { get; set; }
    }
}
