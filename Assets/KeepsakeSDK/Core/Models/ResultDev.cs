using System;
using System.Collections.Generic;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class ResultDev
    {
        public Effects Effects { get; set; }
        public List<object> Events { get; set; }
        public List<ResultDev> Results { get; set; }
    }
}
