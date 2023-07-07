using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class KioskContent : Content
    {
        public KioskFields fields { get; set; }
    }
}