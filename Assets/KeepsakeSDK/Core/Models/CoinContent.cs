using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class CoinContent : Content
    {
        public CoinFields fields { get; set; }
    }
}