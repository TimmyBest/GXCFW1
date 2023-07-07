using System;
using System.Collections.Generic;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class Effects
    {
        public string MessageVersion { get; set; }
        public Status Status { get; set; }
        public string ExecutedEpoch { get; set; }
        public GasUsed GasUsed { get; set; }
        public List<ModifiedAtVersion> ModifiedAtVersions { get; set; }
        public string TransactionDigest { get; set; }
        public List<Created> Created { get; set; }
        public List<Mutated> Mutated { get; set; }
        public GasObject GasObject { get; set; }
        public List<string> Dependencies { get; set; }
    }
}
