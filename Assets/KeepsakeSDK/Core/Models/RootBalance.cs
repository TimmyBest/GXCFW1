using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class RootBalance : RootOwned
    {
        public ResultBalance Result { get; set; }
    }
}