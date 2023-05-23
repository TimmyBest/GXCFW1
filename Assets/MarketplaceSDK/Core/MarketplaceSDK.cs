using MarketplaceSDK.Https;
using MarketplaceSDK.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
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
            var customAttribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("OnSearchListing");

            string response = await httpClient.PostRequest(customAttribute.Url);
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response);
            return myDeserializedClass;
        }
    }
}