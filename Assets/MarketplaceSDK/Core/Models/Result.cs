using Newtonsoft.Json;
using System;

namespace MarketplaceSDK.Core.Models
{
    [Serializable]
    public class Result
    {
        public Result(float size, float speed, string edgeColor, string sideColor)
        {
            Nft = new Nft();
            Nft.Fields = new Field();

            Nft.Fields.Size = size;
            Nft.Fields.Speed = speed;
            Nft.Fields.EdgeColor = edgeColor;
            Nft.Fields.SideColor = sideColor;
        }

        [JsonProperty("_id")]
        public string Id { get; set; }
        [JsonProperty("listing_object_id")]
        public string ListingObjectId { get; set; }
        public string NftObjectId { get; set; }
        public string Market { get; set; }
        public Nft Nft { get; set; }
        public string NftCollection { get; set; }
        public string ObjectType { get; set; }
        public string Seller { get; set; }
        [JsonProperty("sale_price")]
        public double SalePrice { get; set; }
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
        [JsonProperty("seller_kiosk")]
        public string SellerKiosk { get; set; }
    }
}