using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class KeepsakeRoot
    {
        public string Status { get; set; }
        public string Token { get; set; }
        public Data Data { get; set; }
    }
}
