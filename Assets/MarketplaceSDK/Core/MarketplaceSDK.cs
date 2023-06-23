using MarketplaceSDK.Https;
using MarketplaceSDK.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MarketplaceSDK
{
    public sealed class MarketplaceSDK
    {
        private static HttpClient httpClient = new();

        [Http("https://beta-api.keepsake.gg/web/v1/listings/search")]
        public static async Task<Root> OnSearchListing(int salePrice, string nftCollection, string searchName, long gte, long lte)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("OnSearchListing");
            //""sale_price"": {{ ""gte"": {gte}, ""lte"": {lte} }},
            string requestBody = $@"
            {{
                ""sortParams"": {{ ""sale_price"": {salePrice} }},
                ""nft_collection"": ""{nftCollection}"",
                ""searchName"": ""{searchName}"",
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
        public static async Task<RootBalance> GetWalletBalance(string walletId)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("GetWalletBalance");

            string requestBody = $@"{{
                ""jsonrpc"": ""2.0"",
                ""method"": ""suix_getBalance"",
                ""params"": [
                    ""{walletId}"",
                    ""0x2::sui::SUI""
                ],
                ""id"": 1
            }}";

            string response = await httpClient.PostRequest(attribute.Url, requestBody);
            RootBalance session = JsonConvert.DeserializeObject<RootBalance>(response);

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
        public static async Task<string> BuildBuyTransaction(string token, string objectId, string coin, string kiosk)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("BuildBuyTransaction");

            string requestBody = $@"{{
                ""coin"": ""{coin}"",
                ""buyer_kiosk"": ""{kiosk}""
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url + objectId, requestBody, "Authorization", $"Bearer {token}");
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
            Gasless session = JsonConvert.DeserializeObject<Gasless>(response);

            return session.GaslessTx;
        }

        [Http("https://beta-api.keepsake.gg/web/v1/listings/mergeCoins")]
        public static async Task<string> MergeCoins(string token, CoinDataOwned[] multiCoin)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("MergeCoins");

            string requestBody = $@"{{
                ""coins"": [{string.Join(", ", multiCoin.Select(obj => $"\"{obj.Data.ObjectId}\""))}]
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "Authorization", $"Bearer {token}");
            Gasless session = JsonConvert.DeserializeObject<Gasless>(response);

            return session.GaslessTx;
        }

        [Http("https://api.shinami.com/node/v1/sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6")]
        public static async Task<ResultDev> DevInspectTransactionBlock(string walletId, string gaslessTx)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("DevInspectTransactionBlock");

            string requestBody = $@"{{ ""jsonrpc"":""2.0"", 
              ""method"":""sui_devInspectTransactionBlock"",
              ""params"":[""{walletId}"",
                        ""{gaslessTx}""],
            
              ""id"":1}}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "X-API-Key", "a918c93e7b80b633903319d9c6a4c146");
            RootDev session = JsonConvert.DeserializeObject<RootDev>(response);

            return session.Result;
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

        [Http("https://api.shinami.com/node/v1/sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6")]
        public static async Task<RootDynamic> GetDynamicField(string kiosk)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("GetDynamicField");

            string requestBody = $@"{{
              ""jsonrpc"": ""2.0"",
              ""method"": ""suix_getDynamicFields"",
              ""params"": [
                ""{kiosk}""
              ],
              ""id"": 1
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "X-API-Key", "a918c93e7b80b633903319d9c6a4c146");
            RootDynamic session = JsonConvert.DeserializeObject<RootDynamic>(response);

            return session;
        }

        [Http("https://api.shinami.com/node/v1/sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6")]
        public static async Task<RootMulti> GetMultiObjects(string[] multiObjects)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("GetMultiObjects");

            string requestBody = $@"{{ ""jsonrpc"":""2.0"", 
                ""method"":""sui_multiGetObjects"",
                ""params"":[[{string.Join(", ", multiObjects.Select(obj => $"\"{obj}\""))}],
                {{
                        ""showType"": true,
                        ""showOwner"": true,
                        ""showPreviousTransaction"": true,
                        ""showDisplay"": false,
                        ""showContent"": true,
                        ""showBcs"": false,
                        ""showStorageRebate"": true
                }}],     
                ""id"":1}}";
            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "X-API-Key", "a918c93e7b80b633903319d9c6a4c146");
            RootMulti root = JsonConvert.DeserializeObject<RootMulti>(response);

            return root;
        }

        [Http("https://beta-api.keepsake.gg/web/v1/listings/my")]
        public static async Task<Root> GetMyListing(string token)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("GetMyListing");

            string requestBody = "";

            string response = await httpClient.GetRequestWithAuthorization(attribute.Url, requestBody, "Authorization", $"Bearer {token}");
            Root root = JsonConvert.DeserializeObject<Root>(response);

            return root;
        }

        [Http("https://beta-api.keepsake.gg/web/v1/listings/unlist/")]
        public static async Task<string> UnlistAsset(string objectId, string token)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("UnlistAsset");

            string requestBody = "";

            string response = await httpClient.GetRequestWithAuthorization(attribute.Url + objectId, requestBody, "Authorization", $"Bearer {token}");
            Gasless session = JsonConvert.DeserializeObject<Gasless>(response);

            return session.GaslessTx;
        }

        [Http("https://beta-api.keepsake.gg/web/v1/collections/id/")]
        public static async Task<RootObjectType> GetObjectType(string collectionId)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("GetObjectType");

            string requestBody = "";

            string response = await httpClient.GetRequest(attribute.Url + collectionId, requestBody);
            RootObjectType root = JsonConvert.DeserializeObject<RootObjectType>(response);

            return root;
        }

        [Http("https://beta-api.keepsake.gg/web/v1/listings/make_ob_kiosk")]
        public static async Task<string> MakeObKiosk(string token)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<MarketplaceSDK>("MakeObKiosk");

            string requestBody = "";

            string response = await httpClient.GetRequestWithAuthorization(attribute.Url, requestBody, "Authorization", $"Bearer {token}");
            Gasless session = JsonConvert.DeserializeObject<Gasless>(response);

            return session.GaslessTx;
        }
    }
}