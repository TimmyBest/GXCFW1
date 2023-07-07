using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class NameDynamic
    {
        public string Type { get; set; }
        public Value Value { get; set; }
    }
}