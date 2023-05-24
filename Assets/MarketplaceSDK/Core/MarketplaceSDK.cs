using MarketplaceSDK.Https;
using MarketplaceSDK.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MarketplaceSDK
{
    public sealed class MarketplaceSDK
    {
        private static HttpClient httpClient = new();

        [Http("https://beta-api.keepsake.gg/web/v1/listings/search")]
        public static async Task<Root> OnSearchListing()
        {
            var attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("OnSearchListing");

            string response = await httpClient.PostRequest(attribute.Url);
            Root root = JsonConvert.DeserializeObject<Root>(response);
            return root;
        }
    }
}