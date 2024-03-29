using Newtonsoft.Json;
using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class Nft
    {
        public string Id { get; set; }
        public string NftCollection { get; set; }
        public int Network { get; set; }
        [JsonProperty("object_id")]
        public string ObjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Favorites { get; set; }
        public Field Fields { get; set; }
        public int V { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}