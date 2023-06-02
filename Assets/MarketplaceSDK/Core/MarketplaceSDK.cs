using MarketplaceSDK.Https;
using MarketplaceSDK.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine;

namespace MarketplaceSDK
{
    public sealed class MarketplaceSDK
    {
        private static HttpClient httpClient = new();

        [Http("https://beta-api.keepsake.gg/web/v1/listings/search")]
        public static async Task<Root> OnSearchListing()
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("OnSearchListing");

            string requestBody = @"
            {
                ""sortParams"": { ""sale_price"": 1 },
                ""nft_collection"": ""6462c8af23a2b24070683fd1"",
                ""searchName"": ""Whacky Cube Smash"",
                ""sale_price"": { ""gte"": 10000000, ""lte"": 10000000000000 },
                ""sale_type"": ""sale"",
                ""featured"": false,
                ""active"": true
            }";

            string response = await httpClient.PostRequest(attribute.Url, requestBody);
            Root root = JsonConvert.DeserializeObject<Root>(response);

            return root;
        }
    }
}