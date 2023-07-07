using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class GasUsed
    {
        public long ComputationCost { get; set; }
        public long StorageCost { get; set; }
        public long StorageRebate { get; set; }
        public long NonRefundableStorageFee { get; set; }
    }
}