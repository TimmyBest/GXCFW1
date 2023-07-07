using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class ResultBalance
    {
        public string CoinType { get; set; }
        public int CoinObjectCount { get; set; }
        public double TotalBalance { get; set; }
        public LockedBalance LockedBalance { get; set; }
    }
}