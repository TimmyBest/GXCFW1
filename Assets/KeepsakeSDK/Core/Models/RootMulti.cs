using System;
using System.Collections.Generic;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class RootMulti : RootOwned
    {
        public List<ResultMulti> Result { get; set; }
    }
}
