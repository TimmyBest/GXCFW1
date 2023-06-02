using System;

namespace MarketplaceSDK.Models
{
    [Serializable]
    public class Creator
    {
        public string Id { get; set; }
        public string AccountAddress { get; set; }
        public int Followers { get; set; }
        public bool Flagged { get; set; }
        public string Bio { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
    }
}