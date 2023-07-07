using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class RootDynamic : RootOwned
    {
        public ResultDynamic Result { get; set; }
    }
}
