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

        [Http("https://api.shinami.com/key/v1/")]
        public static async Task<string> OnCreateSession()
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("OnCreateSession");

            string requestBody = "{\"jsonrpc\":\"2.0\", \"method\":\"shinami_key_createSession\", \"params\":[\"{{secret}}\"], \"id\":1}";
            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "X-API-Key", "sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6");

            Session session = JsonConvert.DeserializeObject<Session>(response);
            return session.Result;
        }

        [Http("https://api.shinami.com/wallet/v1")]
        public static async Task<string> OnCreateWallet(string sessionToken, string user)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("OnCreateWallet");

            string requestBody = $@"{{ ""jsonrpc"":""2.0"", ""method"":""shinami_wal_createWallet"", ""params"":[""{user}"", ""{sessionToken}""], ""id"":1 }}";
            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "X-API-Key", "sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6");

            Session session = JsonConvert.DeserializeObject<Session>(response);
            return session.Result;
        }

        [Http("https://api.shinami.com/wallet/v1")]
        public static async Task<string> GetWallet(string user)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("GetWallet");

            string requestBody = $@"{{ ""jsonrpc"":""2.0"", ""method"":""shinami_wal_getWallet"", ""params"":[""{user}""], ""id"":1 }}";
            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "X-API-Key", "sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6");

            return response;
        }

        [Http("https://api.shinami.com/wallet/v1")]
        public static async Task<string> SignPersonalMessage(string walletId, string sessionToken)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("SignPersonalMessage");
            string message = "a2VlcHNha2UuaW86OmxvZ2luOjoxNjg2MDk4OTU3MTY5";
            string requestBody = $@"{{
                ""jsonrpc"": ""2.0"",
                ""method"": ""shinami_wal_signPersonalMessage"",
                ""params"": [
                    ""{walletId}"",
                    ""{sessionToken}"",
                    ""{message}""
                ],
                ""id"": 1
            }}";
            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "X-API-Key", "sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6");
            Session session = JsonConvert.DeserializeObject<Session>(response);

            return session.Result;
        }
    }
}