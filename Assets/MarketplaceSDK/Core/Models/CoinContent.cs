using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class CoinContent : Content
    {
        public CoinFields fields { get; set; }
    }
}