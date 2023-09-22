using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class DataGetObject
    {
        public string ObjectId { get; set; }
        public string Version { get; set; }
        public string Digest { get; set; }

        public ContentGetObject Content { get; set; }
    }
}
