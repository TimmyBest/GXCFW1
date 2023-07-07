using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class DataDynamic
    {
        public NameDynamic Name { get; set; }
        public string BcsName { get; set; }
        public string Type { get; set; }
        public string ObjectType { get; set; }
        public string ObjectId { get; set; }
        public int Version { get; set; }
        public string Digest { get; set; }
    }
}