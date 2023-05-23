using System;

namespace MarketplaceSDK.Models
{
    public class Result
    {
        public string Id { get; set; }
        public string ListingObjectId { get; set; }
        public string NftObjectId { get; set; }
        public string Market { get; set; }
        public Nft Nft { get; set; }
        public string NftCollection { get; set; }
        public string ObjectType { get; set; }
        public string Seller { get; set; }
        public object SalePrice { get; set; }
        public string SaleToken { get; set; }
        public string SaleType { get; set; }
        public object SoldAt { get; set; }
        public bool Active { get; set; }
        public bool Hidden { get; set; }
        public bool Featured { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int V { get; set; }
        public Collection Collection { get; set; }
        public Creator Creator { get; set; }
        public int TotalCount { get; set; }
        public string OrderBook { get; set; }
        public string SellerKiosk { get; set; }
    }
}