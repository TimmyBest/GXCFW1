using System;
using System.Collections.Generic;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class RootGetObject : RootOwned
    {
        public ResultGetObject Result { get; set; }
    }
}


