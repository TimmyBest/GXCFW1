using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class RootDev : RootOwned
    {
        public ResultDev Result { get; set; }
    }
}
