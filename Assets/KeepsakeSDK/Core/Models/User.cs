using System;
using System.Collections.Generic;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class User
    {
        public string Id { get; set; }
        public int Banner { get; set; }
        public object AvatarUrl { get; set; }
        public string AccountPubkey { get; set; }
        public List<object> FavoriteNfts { get; set; }
        public List<object> FavoriteCollections { get; set; }
        public List<object> Following { get; set; }
        public int Followers { get; set; }
        public int RoleId { get; set; }
        public bool Flagged { get; set; }
        public string AccountAddress { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int __v { get; set; }
    }
}