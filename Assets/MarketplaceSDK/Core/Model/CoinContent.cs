using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class CoinContent : Content
    {
        public CoinFields fields { get; set; }
    }
}