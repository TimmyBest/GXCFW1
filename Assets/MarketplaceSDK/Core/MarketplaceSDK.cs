using MarketplaceSDK.Https;
using MarketplaceSDK.Models;
using Newtonsoft.Json;
using System;
using System.Text;
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

            string requestBody = $@"
            {{
                ""sortParams"": {{ ""sale_price"": 1 }},
                ""nft_collection"": ""6462c8af23a2b24070683fd1"",
                ""searchName"": ""Whacky Cube Smash"",
                ""sale_price"": {{ ""gte"": 10000000, ""lte"": 10000000000000 }},
                ""sale_type"": ""sale"",
                ""featured"": false,
                ""active"": true
            }}";

            string response = await httpClient.PostRequest(attribute.Url, requestBody);
            Root root = JsonConvert.DeserializeObject<Root>(response);

            return root;
        }

        [Http("https://api.shinami.com/key/v1/")]
        public static async Task<string> OnCreateSession(string secretKey)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("OnCreateSession");

            string requestBody = $@"{{""jsonrpc"":""2.0"", ""method"":""shinami_key_createSession"", ""params"":[""{secretKey}""], ""id"":1}}";
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

            Session session = JsonConvert.DeserializeObject<Session>(response);
            return session.Result;
        }

        [Http("https://api.shinami.com/wallet/v1")]
        public static async Task<string> SignPersonalMessage(string nickname, string timestamp, string sessionToken)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("SignPersonalMessage");

            string message = "keepsake.io::login::" + timestamp;
            byte[] byteArray = Encoding.UTF8.GetBytes(message);

            string base64Message = Convert.ToBase64String(byteArray);

            string requestBody = $@"{{
                ""jsonrpc"": ""2.0"",
                ""method"": ""shinami_wal_signPersonalMessage"",
                ""params"": [
                    ""{nickname}"",
                    ""{sessionToken}"",
                    ""{base64Message}""
                ],
                ""id"": 1
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "X-API-Key", "sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6");
            Session session = JsonConvert.DeserializeObject<Session>(response);

            return session.Result;
        }

        [Http("https://beta-api.keepsake.gg/web/v1/users/sign_up")]
        public static async Task<string> LoginToKeepsake(string signature, string timestamp)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("LoginToKeepsake");
            string requestBody = $@"{{
                ""data"": ""keepsake.io::login::{timestamp}"",
                ""signedMessageResponse"":  ""{signature}""
            }}";

            string response = await httpClient.PostRequest(attribute.Url, requestBody);
            KeepsakeRoot session = JsonConvert.DeserializeObject<KeepsakeRoot>(response);

            return session.token;
        }

        [Http("https://api.shinami.com/node/v1/sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6")]
        public static async Task<string> GetOwnedObjects(string walletId)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("GetOwnedObjects");

            string requestBody = $@"{{
              ""jsonrpc"": ""2.0"",
              ""method"": ""suix_getOwnedObjects"",
              ""params"": [
                ""{walletId}"",
                {{
                    ""filter"": {{
                        ""StructType"": ""0x644fc9ec75f623dcc68338c4cc8d4fcbc2fd8e442a578cbe68a13acb1ee6f363::keepsake_nft::KEEPSAKE""
                }},
                  ""options"": {{
                      ""showDisplay"": true,
                      ""showContent"": true,
                      ""showType"": true
                    }}
                }}
              ],
              ""id"": 1
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "X-API-Key", "a918c93e7b80b633903319d9c6a4c146");
            //Session session = JsonConvert.DeserializeObject<Session>(response);

            return response;
        }

        [Http("https://api.shinami.com/node/v1/sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6")]
        public static async Task<CoinRootOwned> GetOwnedObjectCoins(string walletId)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("GetOwnedObjectCoins");

            string requestBody = $@"{{""jsonrpc"":""2.0"",""method"":""suix_getOwnedObjects"",""params"":[""{walletId}"",
                {{""filter"":{{""StructType"":""0x2::coin::Coin<0x2::sui::SUI>""}},
                ""options"":{{""showDisplay"":true,""showContent"":true,""showType"":true}}}}],""id"":1}}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "X-API-Key", "a918c93e7b80b633903319d9c6a4c146");
            CoinRootOwned session = JsonConvert.DeserializeObject<CoinRootOwned>(response);

            return session;
        }

        [Http("https://api.shinami.com/node/v1/sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6")]
        public static async Task<KioskRootOwned> GetOwnedObjectKiosk(string walletId)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("GetOwnedObjectKiosk");

            string structType = "0xd0a449fc6b27195df90aff0076cdd84dd43f600235b23144f2422741b1bb86a8::ob_kiosk::OwnerToken";

            string requestBody = $@"{{
              ""jsonrpc"": ""2.0"",
              ""method"": ""suix_getOwnedObjects"",
              ""params"": [
                ""{walletId}"",
                {{
                  ""filter"": {{
                    ""StructType"": ""{structType}""
                  }},
                  ""options"": {{
                      ""showDisplay"": true,
                      ""showContent"": true,
                      ""showType"": true
                  }}
                }}
              ],
              ""id"": 1
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "X-API-Key", "a918c93e7b80b633903319d9c6a4c146");
            KioskRootOwned session = JsonConvert.DeserializeObject<KioskRootOwned>(response);

            return session;
        }

        [Http("https://beta-api.keepsake.gg/web/v1/listings/buy/")]
        public static async Task<string> BuildBuyTransaction(string token, string listingId, string coin, string kiosk)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("BuildBuyTransaction");

            string requestBody = $@"{{
                ""coin"": ""{coin}"",
                ""buyer_kiosk"": ""{kiosk}""
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url + listingId, requestBody, "Authorization", $"Bearer {token}");
            Debug.Log(response);
            Gasless session = JsonConvert.DeserializeObject<Gasless>(response);

            return session.GaslessTx;
        }

        [Http("https://beta-api.keepsake.gg/web/v1/listings/sell")]
        public static async Task<string> BuildSellTransaction(string token, string nftId, string suiPrice, string kiosk)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("BuildSellTransaction");

            string requestBody = $@"{{
                ""nft_id"": ""{nftId}"",
                ""sui_price"": ""{suiPrice}"",
                ""seller_kiosk"": ""{kiosk}""
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "Authorization", $"Bearer {token}");
            //Session session = JsonConvert.DeserializeObject<Session>(response);

            return response;
        }

        [Http("https://api.shinami.com/node/v1/sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6")]
        public static async Task<string> DevInspectTransactionBlock(string walletId, string gaslessTx)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("DevInspectTransactionBlock");

            string requestBody = $@"{{ ""jsonrpc"":""2.0"", 
              ""method"":""sui_devInspectTransactionBlock"",
              ""params"":[""{walletId}"",
                        ""{gaslessTx}""],
            
              ""id"":1}}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "X-API-Key", "a918c93e7b80b633903319d9c6a4c146");
            //Session session = JsonConvert.DeserializeObject<Session>(response);

            return response;
        }

        [Http("https://api.shinami.com/wallet/v1")]
        public static async Task<string> ExecuteGaslessTransactionBlock(string nickname, string sessionId, string gaslessTx, long cost)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("ExecuteGaslessTransactionBlock");

            string requestBody = $@"{{
                ""jsonrpc"": ""2.0"",
                ""method"": ""shinami_wal_executeGaslessTransactionBlock"",
                ""params"": [
                    ""{nickname}"",
                    ""{sessionId}"",
                    ""{gaslessTx}"",
                    {cost},
                    {{
                        ""showRawInput"": false,
                        ""showInput"": false,
                        ""showEffects"": false,
                        ""showEvents"": false,
                        ""showObjectChanges"": false,
                        ""showBalanceChanges"": false
                    }},
                    ""WaitForLocalExecution""
                ],
                ""id"": 1
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "X-API-Key", "sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6");
            //Session session = JsonConvert.DeserializeObject<Session>(response);

            return response;
        }
    }
}