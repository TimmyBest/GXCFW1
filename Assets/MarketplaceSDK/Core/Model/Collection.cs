using System;
using System.Collections.Generic;

namespace MarketplaceSDK.Models
{
    public class Collection
    {
        public string Id { get; set; }
        public string ObjectId { get; set; }
        public string CollectionObjectId { get; set; }
        public string ModuleName { get; set; }
        public string NftName { get; set; }
        public string Standard { get; set; }
        public bool Keepsake { get; set; }
        public string MintObjectId { get; set; }
        public string TransferPolicy { get; set; }
        public string TransferPolicyCap { get; set; }
        public string PublisherObjectId { get; set; }
        public string Creator { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Favorites { get; set; }
        public List<string> Tags { get; set; }
        public List<Field> Fields { get; set; }
        public bool Active { get; set; }
        public string ReviewStatus { get; set; }
        public bool Hidden { get; set; }
        public bool Featured { get; set; }
        public List<Royalty> Royalties { get; set; }
        public int Floor { get; set; }
        public int Listed { get; set; }
        public long Volume { get; set; }
        public int TotalSales { get; set; }
        public int Owners { get; set; }
        public int NftCount { get; set; }
        public string LogoImage { get; set; }
        public string FeaturedImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int V { get; set; }
        public bool? ObEnabled { get; set; }
        public List<ObRoyalty> ObRoyalties { get; set; }
        public string ObAllowList { get; set; }
        public string ObTransferPolicy { get; set; }
        public string ObTransferPolicyCap { get; set; }
        public string OrderBook { get; set; }
    }
}