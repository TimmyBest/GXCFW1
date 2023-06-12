using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class Session
    {
        public string JsonRpc { get; set; }
        public string Result { get; set; }
        public int Id { get; set; }
    }
}
