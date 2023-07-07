using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class ResultOwned
    {
        public string NextCursor { get; set; }
        public bool HasNextPage { get; set; }
    }
}
