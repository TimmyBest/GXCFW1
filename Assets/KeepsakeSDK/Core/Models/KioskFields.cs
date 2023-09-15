using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class KioskFields : Fields
    {
        public string Kiosk { get; set; }
        public string Owner { get; set; }

        public KioskCap Cap { get; set; }
    }
}
