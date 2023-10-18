using KeepsakeSDK.Core.Tools;
using KeepsakeSDK.Core.Enums;
using KeepsakeSDK.Core.Https;
using KeepsakeSDK.Core.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KeepsakeSDK
{
    public sealed class KeepsakeSDK
    {

        private static HttpClient httpClient = new HttpClient();
        public static async Task<string> AuthorizationKey()
        {
            string authorization_key = "X-API-KEY";
            return authorization_key;
        }

        /// <summary>
        /// Authorization value is provided to you by Keepsake and is unique to all games. 
        /// You only need to change the authorization_value param here.
        public static async Task<string> AuthorizationValue()
        {
            string authorization_value = "API_KEY_1";
            return authorization_value;
        }

        

        /// <summary>
        /// For security purposes, you must generate a session token before you create a wallet, sign transactions, or execute transactions. Session tokens expire after 10 minutes.
        /// </summary>
        /// <param name="secretKey">Used to encrypt a user's wallet private key. Typically, this would tie to your app's registration and authentication.</param>
        /// <returns>Session token - Used to create a wallet, sign transactions or execute transactions.</returns>
        [Http("https://integrated.keepsake.gg/web/v1/sdk/createSession")]
        public static async Task<string> OnCreateSession(string secretKey)
        {
            try
            {
                HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("OnCreateSession");

                string authorization_key = await AuthorizationKey();
                string authorization_value = await AuthorizationValue();

                string requestBody = $@"{{""jsonrpc"":""2.0"", ""method"":""shinami_key_createSession"", ""params"":[""{secretKey}""], ""id"":1}}";
                string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, authorization_key, authorization_value);

                Session session = JsonConvert.DeserializeObject<Session>(response);
                Debug.Log("INFO: " + session.Result);
                return session.Result;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while creating session: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Programmatically generates a unique wallet for a user that is Sui network agnostic.
        /// </summary>
        /// <param name="nickname">User nickname.</param>
        /// <param name="secretKey">Used to encrypt a user's wallet private key. Typically, this would tie to your app's registration and authentication.</param>
        /// <param name="sessionToken">The session token that we generated. Session tokens expire after 10 minutes.</param>
        /// <returns>Wallet address - Sui wallet address created for the user.</returns>
        [Http("https://integrated.keepsake.gg/web/v1/sdk/createWallet")]
        public static async Task<string> OnCreateWallet(string sessionToken, string nickname)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("OnCreateWallet");

            string authorization_key = await AuthorizationKey();
            string authorization_value = await AuthorizationValue();

            string requestBody = $@"{{ ""jsonrpc"":""2.0"", ""method"":""shinami_wal_createWallet"", ""params"":[""{nickname}"", ""{sessionToken}""], ""id"":1 }}";
            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, authorization_key, authorization_value);
            Debug.Log("INFO create wallet: " + response);
            Session session = JsonConvert.DeserializeObject<Session>(response);

            return session.Result;
        }

        /// <summary>
        /// Retrieve a user's wallet address based on their ID.
        /// </summary>
        /// <param name="walletId">An arbitrary and unique ID for the wallet. Can be based on your internal user IDs.</param>
        /// <returns>Wallet address - Sui wallet address created for the user.</returns>
        [Http("https://integrated.keepsake.gg/web/v1/sdk/getWallet")]
        public static async Task<string> GetWallet(string walletId)
        {
            try
            {
                HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("GetWallet");

                string authorization_key = await AuthorizationKey();
                string authorization_value = await AuthorizationValue();

                string requestBody = $@"{{ ""jsonrpc"":""2.0"", ""method"":""shinami_wal_getWallet"", ""params"":[""{walletId}""], ""id"":1 }}";
                string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, authorization_key, authorization_value);
                
                if (string.IsNullOrWhiteSpace(response))
                {
                    Debug.LogError("GetWallet: Empty response received.");
                    return null;
                }

                Session session = JsonConvert.DeserializeObject<Session>(response);

                if (session == null || string.IsNullOrWhiteSpace(session.Result))
                {
                    Debug.LogError("GetWallet: Invalid session data.");
                    return null;
                }
                
                return session.Result;
            }
            catch (Exception ex)
            {
                Debug.LogError($"GetWallet Error: {ex.Message}");
                return null;
            }
        }


        /// <summary>
        /// Signs a fully constructed transaction block. This is a low level API - it requires integration with Gas Station API and Sui API for transaction sponsorship (if needed) and execution. This method gives you more control over how you submit transactions to Sui.
        /// </summary>
        /// <param name="walletId">An arbitrary and unique ID for the wallet. Can be based on your internal user IDs.</param>
        /// <param name="timestamp">Arbitrary message bytes, as Base64 encoded string.</param>
        /// <param name="sessionToken">The session token that we generated. Session tokens expire after 10 minutes.</param>
        /// <returns>Signature - Base64 encoded signature, signed by the wallet key</returns>
        [Http("https://integrated.keepsake.gg/web/v1/sdk/signPersonalMessage")]
        public static async Task<string> SignPersonalMessage(string walletId, string timestamp, string sessionToken)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("SignPersonalMessage");

            string authorization_key = await AuthorizationKey();
            string authorization_value = await AuthorizationValue();

            string message = "keepsake.io::login::" + timestamp;
            byte[] byteArray = Encoding.UTF8.GetBytes(message);

            string base64Message = Convert.ToBase64String(byteArray);

            bool wrapBsc = true;

            string requestBody = $@"{{
                ""jsonrpc"": ""2.0"",
                ""method"": ""shinami_wal_signPersonalMessage"",
                ""params"": [
                    ""{walletId}"",
                    ""{sessionToken}"",
                    ""{base64Message}"",
                    ""{wrapBsc}""
                ],
                ""id"": 1
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, authorization_key, authorization_value);
            Session session = JsonConvert.DeserializeObject<Session>(response);

            if (session.Result == null) { return null; }

            return session.Result;
        }

        /// <summary>
        /// Return the list of objects owned by an address.
        /// </summary>
        /// <param name="walletId">The wallet address.</param>
        [Http("https://api.shinami.com/node/v1/sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6")]
        public static async Task<string> GetOwnedObjects(string walletId)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("GetOwnedObjects");

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

        /// <summary>
        /// Return a list of Coin objects by type owned by an address.
        /// </summary>
        /// <param name="walletId">The wallet address.</param>
        [Http("https://api.shinami.com/node/v1/sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6")]
        public static async Task<CoinRootOwned> GetOwnedObjectCoins(string walletId)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("GetOwnedObjectCoins");

            string authorization_key = await AuthorizationKey();
            string authorization_value = await AuthorizationValue();

            string requestBody = $@"{{""jsonrpc"":""2.0"",""method"":""suix_getOwnedObjects"",""params"":[""{walletId}"",
                {{""filter"":{{""StructType"":""0x2::coin::Coin<0x2::sui::SUI>""}},
                ""options"":{{""showDisplay"":true,""showContent"":true,""showType"":true}}}}],""id"":1}}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, authorization_key, authorization_value);
            CoinRootOwned session = JsonConvert.DeserializeObject<CoinRootOwned>(response);

            return session;
        }

        /// <summary>
        /// Return the total Coin balance for each coin type owned by an address.
        /// </summary>
        /// <param name="walletId">The wallet address.</param>
        [Http("https://integrated.keepsake.gg/web/v1/sdk/getWalletBalance")]
        public static async Task<RootBalance> GetWalletBalance(string walletId)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("GetWalletBalance");

            string authorization_key = await AuthorizationKey();
            string authorization_value = await AuthorizationValue();

            string requestBody = $@"{{
                ""jsonrpc"": ""2.0"",
                ""method"": ""suix_getBalance"",
                ""params"": [
                    ""{walletId}"",
                    ""0x2::sui::SUI""
                ],
                ""id"": 1
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, authorization_key, authorization_value);
            RootBalance session = JsonConvert.DeserializeObject<RootBalance>(response);

            return session;
        }

        /// <summary>
        /// Return the Mysten Kiosk package information. Only used for the Sui Kiosk
        /// </summary>
        [Http("https://beta-api.keepsake.gg/web/v1/services/settings")]
        public static async Task<Package> SuiPackageInformation()
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("SuiPackageInformation");
            string requestBody = "";

            string response = await httpClient.GetRequest(attribute.Url, requestBody);
            Package session = JsonConvert.DeserializeObject<Package>(response);

            return session;
        }

        /// <summary>
        /// Return a list of Kiosk objects by type owned by an address.
        /// </summary>
        /// <param name="walletId">The wallet address.</param>
        [Http("https://api.shinami.com/node/v1/sui_testnet_a3990d6eb0bd26173a4a5e39a7961bc6")]
        public static async Task<KioskRootOwned> GetOwnedObjectKiosk(string walletId)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("GetOwnedObjectKiosk");

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

        /// <summary>
        /// Return a list of Sui Kiosk objects by type owned by an address.
        /// </summary>
        /// <param name="walletId">The wallet address.</param>
        [Http("https://integrated.keepsake.gg/web/v1/sdk/getOwnedObjects")]
        public static async Task<KioskRootOwned> GetOwnedObjectSuiKiosk(string walletId)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("GetOwnedObjectSuiKiosk");

            string authorization_key = await AuthorizationKey();
            string authorization_value = await AuthorizationValue();

            // string structType = "0x2::kiosk::KioskOwnerCap";
            Package package = await  SuiPackageInformation();
            // string structType = package.Packages.mysten_kiosk + "::personal_kiosk::PersonalKioskCap";
            string structType = package.ObjectIds.personal_kiosk + "::personal_kiosk::PersonalKioskCap";
            Debug.Log("package info" + structType);

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

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, authorization_key, authorization_value);
            KioskRootOwned session = JsonConvert.DeserializeObject<KioskRootOwned>(response);
            return session;
        }

        /// <summary>
        /// Runs the transaction in dev-inspect mode, which allows for nearly any transaction (or Move call) with any arguments. Detailed results are provided, including both the transaction effects and any return values.
        /// </summary>
        /// <param name="walletId">The wallet address.</param>
        /// <param name="tx_bytes">Transaction data bytes, as base-64 encoded string.</param>
        [Http("https://integrated.keepsake.gg/web/v1/sdk/devInspectTransactionBlock")]
        public static async Task<ResultDev> DevInspectTransactionBlock(string walletId, string tx_bytes)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("DevInspectTransactionBlock");

            string authorization_key = await AuthorizationKey();
            string authorization_value = await AuthorizationValue();

            string requestBody = $@"{{ ""jsonrpc"":""2.0"", 
              ""method"":""sui_devInspectTransactionBlock"",
              ""params"":[""{walletId}"",
                        ""{tx_bytes}""],
            
              ""id"":1}}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, authorization_key, authorization_value);
            RootDev session = JsonConvert.DeserializeObject<RootDev>(response);

            return session.Result;
        }

        /// <summary>
        /// Execute a transaction using the transaction data and signature(s).
        /// </summary>
        /// <param name="nickname">The caller's Sui address.</param>
        /// <param name="sessionToken">The session token that we generated. Session tokens expire after 10 minutes.</param>
        /// <param name="tx_bytes">BCS serialized transaction data bytes without its type tag, as base-64 encoded string.</param>
        /// <param name="cost">Price.</param>
        [Http("https://integrated.keepsake.gg/web/v1/sdk/executeGaslessTransactionBlock")]
        public static async Task<string> ExecuteGaslessTransactionBlock(string nickname, string sessionToken, string tx_bytes, long cost)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("ExecuteGaslessTransactionBlock");

            string authorization_key = await AuthorizationKey();
            string authorization_value = await AuthorizationValue();

            string requestBody = $@"{{
                ""jsonrpc"": ""2.0"",
                ""method"": ""shinami_wal_executeGaslessTransactionBlock"",
                ""params"": [
                    ""{nickname}"",
                    ""{sessionToken}"",
                    ""{tx_bytes}"",
                    {cost},
                    {{
                        ""showRawInput"": false,
                        ""showInput"": false,
                        ""showEffects"": true,
                        ""showEvents"": false,
                        ""showObjectChanges"": false,
                        ""showBalanceChanges"": false
                    }},
                    ""WaitForLocalExecution""
                ],
                ""id"": 1
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, authorization_key, authorization_value);
            RootTransaction root = JsonConvert.DeserializeObject<RootTransaction>(response);
            if (root.Result.Effects.Status.status == null)
                return "failure";

            return root.Result.Effects.Status.status;
        }

        /// <summary>
        /// Return the dynamic field object information for a specified object.
        /// </summary>
        /// <param name="kiosk">The kiosk address.</param>
        [Http("https://integrated.keepsake.gg/web/v1/sdk/getDynamicField")]
        public static async Task<RootDynamic> GetDynamicField(string kiosk)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("GetDynamicField");

            string authorization_key = await AuthorizationKey();
            string authorization_value = await AuthorizationValue();

            string requestBody = $@"{{
              ""jsonrpc"": ""2.0"",
              ""method"": ""suix_getDynamicFields"",
              ""params"": [
                ""{kiosk}""
              ],
              ""id"": 1
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, authorization_key, authorization_value);
            RootDynamic session = JsonConvert.DeserializeObject<RootDynamic>(response);

            return session;
        }

        /// <summary>
        /// Return the object data for a list of objects.
        /// </summary>
        /// <param name="multiObjects">The IDs of the queried objects.</param>
        [Http("https://integrated.keepsake.gg/web/v1/sdk/getMultiObjects")]
        public static async Task<RootMulti> GetMultiObjects(string[] multiObjects)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("GetMultiObjects");

            string authorization_key = await AuthorizationKey();
            string authorization_value = await AuthorizationValue();

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
            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, authorization_key, authorization_value);
            RootMulti root = JsonConvert.DeserializeObject<RootMulti>(response);

            return root;
        }

        /// <summary>
        /// Return a list of NFT objects.
        /// </summary>
        /// <param name="salePrice">Price per NFT.</param>
        /// <param name="nftCollection">Address NFT Collection.</param>
        /// <param name="searchName">Name NFT Collection.</param>
        [Http("https://beta-api.keepsake.gg/web/v1/listings/search")]
        public static async Task<string> OnSearchListing(int salePrice, string nftCollection, string searchName)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("OnSearchListing");
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

            return response;
        }

        /// <summary>
        /// Login to Keepsake.
        /// </summary>
        /// <param name="signature">Signature is committed to the intent message of the transaction data.</param>
        /// <param name="timestamp">Unix time milliseconds.</param>
        /// <returns>Token that can be used for transaction.</returns>
        [Http("https://beta-api.keepsake.gg/web/v1/users/sign_up")]
        public static async Task<string> LoginToKeepsake(string signature, string timestamp)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("LoginToKeepsake");

            string requestBody = $@"{{
                ""data"": ""keepsake.io::login::{timestamp}"",
                ""signedMessageResponse"":  ""{signature}""
            }}";

            string response = await httpClient.PostRequest(attribute.Url, requestBody);
            KeepsakeRoot session = JsonConvert.DeserializeObject<KeepsakeRoot>(response);

            return session.Token;
        }

        /// <summary>
        /// Build transaction to buy NFT.
        /// </summary>
        /// <param name="token">The token that was received upon successful login in Keepsake.</param>
        /// <param name="nftId">NFT id.</param>
        /// <param name="coin">Coin address.</param>
        /// <param name="kiosk">Kiosk address.</param>
        [Http("https://beta-api.keepsake.gg/web/v1/listings/buy/")]
        public static async Task<string> BuildBuyTransaction(string token, string nftId, string coin, string kiosk)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("BuildBuyTransaction");

            string requestBody = $@"{{
                ""coin"": ""{coin}"",
                ""buyer_kiosk"": ""{kiosk}""
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url + nftId, requestBody, "Authorization", $"Bearer {token}");
            Gasless session = JsonConvert.DeserializeObject<Gasless>(response);

            return session.GaslessTx;
        }

        /// <summary>
        /// Build transaction to buy Sui Kiosk NFT.
        /// </summary>
        /// <param name="token">The token that was received upon successful login in Keepsake.</param>
        /// <param name="nftId">NFT id.</param>
        /// <param name="coin">Coin address.</param>
        /// <param name="kiosk">Kiosk address.</param>
        [Http("https://beta-api.keepsake.gg/web/v1/listings/buy/")]
        public static async Task<string> BuildBuySuiKioskTransaction(string token, string nftId, string coin, string kiosk, string kioskCap)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("BuildBuySuiKioskTransaction");

            string requestBody = $@"{{
                ""coin"": ""{coin}"",
                ""buyer_kiosk"": ""{kiosk}"",
                ""buyer_kiosk_cap"": ""{kioskCap}""
            }}";

            Debug.Log("GETTING GAS: " + coin);
            Debug.Log("GETTING GAS: " + kiosk);
            Debug.Log("GETTING GAS: " + kioskCap);

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url + nftId, requestBody, "Authorization", $"Bearer {token}");
            Gasless session = JsonConvert.DeserializeObject<Gasless>(response);
            Debug.Log("GETTING GAS: " + session.GaslessTx);
            return session.GaslessTx;
        }

        /// <summary>
        /// Build transaction to withdraw Sui Kiosk Proceeds.
        /// </summary>
        /// <param name="token">The token that was received upon successful login in Keepsake.</param>
        /// <param name="kiosk">Kiosk address.</param>
        /// <param name="kioskCap">Kiosk Capability object address that authorizes the action to the kiosk</param>
        [Http("https://beta-api.keepsake.gg/web/v1/txns/withdraw/")]
        public static async Task<string> BuildWithdrawalSuiKioskProceeds(string token, string kiosk, string kioskCap)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("BuildWithdrawalSuiKioskProceeds");

            string requestBody = $@"{{
                ""kiosk"": ""{kiosk}"",
                ""kiosk_cap"": ""{kioskCap}""
            }}";

            
            Debug.Log("GETTING GAS: " + kiosk);
            Debug.Log("GETTING GAS: " + kioskCap);

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "Authorization", $"Bearer {token}");
            Gasless session = JsonConvert.DeserializeObject<Gasless>(response);
            Debug.Log("GETTING GAS: " + session.GaslessTx);
            return session.GaslessTx;
        }

        /// <summary>
        /// Build transaction to sell NFT.
        /// </summary>
        /// <param name="token">The token that was received upon successful login in Keepsake.</param>
        /// <param name="nftId">NFT id.</param>
        /// <param name="suiPrice">Price NFT.</param>
        /// <param name="kiosk">Kiosk address.</param>
        [Http("https://beta-api.keepsake.gg/web/v1/listings/sell")]
        public static async Task<string> BuildSellTransaction(string token, string nftId, string suiPrice, string kiosk)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("BuildSellTransaction");

            string requestBody = $@"{{
                ""nft_id"": ""{nftId}"",
                ""sui_price"": ""{suiPrice}"",
                ""seller_kiosk"": ""{kiosk}""
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "Authorization", $"Bearer {token}");
            Gasless session = JsonConvert.DeserializeObject<Gasless>(response);

            return session.GaslessTx;
        }

        /// <summary>
        /// Build transaction to sell NFT with Sui Kiosk.
        /// </summary>
        /// <param name="token">The token that was received upon successful login in Keepsake.</param>
        /// <param name="nftId">NFT id.</param>
        /// <param name="suiPrice">Price NFT.</param>
        /// <param name="kiosk">Kiosk address.</param>
        /// <param name="kioskCap">Kiosk address.</param>
        [Http("https://beta-api.keepsake.gg/web/v1/listings/sell")]
        public static async Task<string> BuildSellSuiKioskTransaction(string token, string nftId, string suiPrice, string kiosk, string kioskCap)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("BuildSellTransaction");

            string requestBody = $@"{{
                ""nft_id"": ""{nftId}"",
                ""sui_price"": ""{suiPrice}"",
                ""seller_kiosk"": ""{kiosk}"",
                ""seller_kiosk_cap"": ""{kioskCap}""
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "Authorization", $"Bearer {token}");
            Gasless session = JsonConvert.DeserializeObject<Gasless>(response);

            return session.GaslessTx;
        }

        /// <summary>
        /// Merge coins.
        /// </summary>
        /// <param name="token">The token that was received upon successful login in Keepsake.</param>
        /// <param name="multiCoin">Array of coins.</param>
        [Http("https://beta-api.keepsake.gg/web/v1/listings/mergeCoins")]
        public static async Task<string> MergeCoins(string token, CoinDataOwned[] multiCoin)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("MergeCoins");

            string requestBody = $@"{{
                ""coins"": [{string.Join(", ", multiCoin.Select(obj => $"\"{obj.Data.ObjectId}\""))}]
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "Authorization", $"Bearer {token}");
            Gasless session = JsonConvert.DeserializeObject<Gasless>(response);

            return session.GaslessTx;
        }

        /// <summary>
        /// Get my listing NFT.
        /// </summary>
        /// <param name="token">The token that was received upon successful login in Keepsake.</param>
        [Http("https://beta-api.keepsake.gg/web/v1/listings/my")]
        public static async Task<Root> GetMyListing(string token)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("GetMyListing");

            string requestBody = "";

            string response = await httpClient.GetRequestWithAuthorization(attribute.Url, requestBody, "Authorization", $"Bearer {token}");
            Root root = JsonConvert.DeserializeObject<Root>(response);

            return root;
        }

        /// <summary>
        /// Unlist asset from sales.
        /// </summary>
        /// <param name="nftId">NFT id.</param>
        /// <param name="token">The token that was received upon successful login in Keepsake.</param>
        [Http("https://beta-api.keepsake.gg/web/v1/listings/unlist/")]
        public static async Task<string> UnlistAssetBuild(string nftId, string token)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("UnlistAssetBuild");

            string requestBody = "";

            string response = await httpClient.GetRequestWithAuthorization(attribute.Url + nftId, requestBody, "Authorization", $"Bearer {token}");
            Gasless session = JsonConvert.DeserializeObject<Gasless>(response);

            return session.GaslessTx;
        }

        /// <summary>
        /// Get object type for comparison
        /// </summary>
        /// <param name="collectionId">Collection NFT ID.</param>
        [Http("https://beta-api.keepsake.gg/web/v1/collections/id/")]
        public static async Task<RootObjectType> GetObjectType(string collectionId)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("GetObjectType");

            string requestBody = "";

            string response = await httpClient.GetRequest(attribute.Url + collectionId, requestBody);
            RootObjectType root = JsonConvert.DeserializeObject<RootObjectType>(response);

            return root;
        }

        /// <summary>
        /// Create a new kiosk.
        /// </summary>
        /// <param name="token">The token that was received upon successful login in Keepsake.</param>
        [Http("https://beta-api.keepsake.gg/web/v1/listings/make_ob_kiosk")]
        public static async Task<string> MakeObKiosk(string token)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("MakeObKiosk");

            string requestBody = "";

            string response = await httpClient.GetRequestWithAuthorization(attribute.Url, requestBody, "Authorization", $"Bearer {token}");
            Gasless session = JsonConvert.DeserializeObject<Gasless>(response);

            return session.GaslessTx;
        }

        /// <summary>
        /// Create a new Sui kiosk.
        /// </summary>
        /// <param name="token">The token that was received upon successful login in Keepsake.</param>
        [Http("https://beta-api.keepsake.gg/web/v1/listings/make_kiosk")]
        public static async Task<string> MakeSuiKiosk(string token)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("MakeSuiKiosk");

            string requestBody = "";

            string response = await httpClient.GetRequestWithAuthorization(attribute.Url, requestBody, "Authorization", $"Bearer {token}");
            Gasless session = JsonConvert.DeserializeObject<Gasless>(response);

            return session.GaslessTx;
        }

        ///<summary>
        ///Get the Sui Kiosk Profit Balance
        ///</summary>
        [Http("https://integrated.keepsake.gg/web/v1/sdk/getObject")]
        public static async Task<RootGetObject> GetObject(string kiosk)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("GetObject");

            string authorization_key = await AuthorizationKey();
            string authorization_value = await AuthorizationValue();

            string requestBody = $@"{{ ""jsonrpc"":""2.0"", 
                ""method"":""sui_getObject"",
                ""params"":[""{kiosk}"",
                {{
                        ""showContent"": true
                }}],     
                ""id"":1}}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, authorization_key, authorization_value);
            RootGetObject root = JsonConvert.DeserializeObject<RootGetObject>(response);

            return root;

        }

        ///<summary>
        ///Used to allows users to get free items out of the "grab bag", similar to an airdrop. Items place in here do not cost any sui
        ///dev can define settings on Keepsake marketplace. Such as amount per wallet, random or specified asset
        ///</summary>
        [Http("https://beta-api.keepsake.gg/web/v1/txns/take_from_bag")]
        public static async Task<string> BuildGiftShopTransaction(string token, string kiosk, string kioskCap, string nftId, string bagId)
        {
            HttpAttribute attribute = HttpAttribute.GetAttributeCustom<KeepsakeSDK>("BuildGiftShopTransaction");

            string requestBody = $@"{{
                ""kiosk"": ""{kiosk}"",
                ""kiosk_cap"": ""{kioskCap}"",
                ""nft_id"": ""{nftId}"",
                ""bag_id"": ""{bagId}""
            }}";

            string response = await httpClient.PostRequestWithAuthorization(attribute.Url, requestBody, "Authorization", $"Bearer {token}");
            Debug.Log("GETTING GAS Response: " + bagId);
            Gasless session = JsonConvert.DeserializeObject<Gasless>(response);
            Debug.Log("GETTING GAS: " + session.GaslessTx);
            return session.GaslessTx;
        }


        /// <summary>
        /// Authorization.
        /// </summary>
        /// <param name="nickname">User nickname.</param>
        /// <param name="secretKey">Used to encrypt a user's wallet private key. Typically, this would tie to your app's registration and authentication.</param>
        public static async Task<StatusAuth> AuthorizationAPI(string nickname, string secretKey)
        {
            string walletId = await GetWallet(nickname);
            if (walletId == null)
            {
                return StatusAuth.WalletNotFound;
            }
            string sessionToken = await OnCreateSession(secretKey);
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = await SignPersonalMessage(nickname, timestamp, sessionToken);
            if (signature == null)
            {
                return StatusAuth.WrongSecretKey;
            }

            return StatusAuth.Success;
        }

        /// <summary>
        /// Sign up account.
        /// </summary>
        /// <param name="nickname">User nickname.</param>
        /// <param name="secretKey">Used to encrypt a user's wallet private key. Typically, this would tie to your app's registration and authentication.</param>
        public static async Task<StatusRegister> SignUpAccount(string nickname, string secretKey)
        {
            if (nickname.Length < 1) return StatusRegister.NicknameEmpty;

            string sessionToken = await OnCreateSession(secretKey);
            string walletId = await OnCreateWallet(sessionToken, nickname);
            if (walletId == null)
            {
                return StatusRegister.WalletExist;
            }

            return StatusRegister.Success;
        }

        /// <summary>
        /// Buy NFT.
        /// </summary>
        /// <param name="nftId">NFT id.</param>
        /// <param name="nickname">User nickname.</param>
        /// <param name="secretKey">Used to encrypt a user's wallet private key. Typically, this would tie to your app's registration and authentication.</param>
        /// <param name="walletId">The wallet address.</param>
        public static async Task<StatusTransaction> BuyNFT(string nftId, string nickname, string secretKey, string walletId)
        {
            string sessionToken = await OnCreateSession(secretKey);
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = await SignPersonalMessage(nickname, timestamp, sessionToken);
            string token = await LoginToKeepsake(signature, timestamp);
            CoinRootOwned coinRoot = await GetOwnedObjectCoins(walletId);
            if (coinRoot.Result.Data.Count > 1)
            {
                string gaslessTxMerge = await MergeCoins(token, coinRoot.Result.Data.ToArray());
                ResultDev rootDevMerge = await DevInspectTransactionBlock(walletId, gaslessTxMerge);
                string responseMerge = await ExecuteGaslessTransactionBlock(nickname, sessionToken, gaslessTxMerge, rootDevMerge.Effects.GasUsed.ComputationCost + rootDevMerge.Effects.GasUsed.StorageCost);
                
                await Task.Delay(3000);

                coinRoot = await GetOwnedObjectCoins(walletId);
            }
            if (coinRoot.Result.Data.Count <= 0)
            {
                return ConvertStringToStatus("Failure");
            }
            KioskRootOwned kioskRoot = await GetOwnedObjectKiosk(walletId);
            if (kioskRoot.Result.Data.Count <= 0)
            {
                await CreateKiosk(nickname, secretKey, walletId);
                return await BuyNFT(nftId, nickname, secretKey, walletId);
            }
            string gaslessTx = await BuildBuyTransaction(token, nftId, ToolExtensions.FindHighestNumber(coinRoot.Result.Data), kioskRoot.Result.Data[0].Data.Display.Data.Kiosk);
            ResultDev rootDev = await DevInspectTransactionBlock(walletId, gaslessTx);
            string response = await ExecuteGaslessTransactionBlock(nickname, sessionToken, gaslessTx, rootDev.Effects.GasUsed.ComputationCost + rootDev.Effects.GasUsed.StorageCost);

            return ConvertStringToStatus(response);
        }

        /// <summary>
        /// Buy Sui Kiosk Based NFT.
        /// </summary>
        /// <param name="nftId">NFT id.</param>
        /// <param name="nickname">User nickname.</param>
        /// <param name="secretKey">Used to encrypt a user's wallet private key. Typically, this would tie to your app's registration and authentication.</param>
        /// <param name="walletId">The wallet address.</param>
        public static async Task<StatusTransaction> BuyNFTSuiKiosk(string nftId, string nickname, string secretKey, string walletId)
        {
            string sessionToken = await OnCreateSession(secretKey);
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = await SignPersonalMessage(nickname, timestamp, sessionToken);
            string token = await LoginToKeepsake(signature, timestamp);
            CoinRootOwned coinRoot = await GetOwnedObjectCoins(walletId);
            if (coinRoot.Result.Data.Count > 1)
            {
                string gaslessTxMerge = await MergeCoins(token, coinRoot.Result.Data.ToArray());
                ResultDev rootDevMerge = await DevInspectTransactionBlock(walletId, gaslessTxMerge);
                string responseMerge = await ExecuteGaslessTransactionBlock(nickname, sessionToken, gaslessTxMerge, rootDevMerge.Effects.GasUsed.ComputationCost + rootDevMerge.Effects.GasUsed.StorageCost);
                
                await Task.Delay(3000);

                coinRoot = await GetOwnedObjectCoins(walletId);
            }
            if (coinRoot.Result.Data.Count <= 0)
            {
                return ConvertStringToStatus("Failure");
            }
            KioskRootOwned kioskRoot = await GetOwnedObjectSuiKiosk(walletId);
            if (kioskRoot.Result.Data.Count <= 0)
            {
                await CreateSuiKiosk(nickname, secretKey, walletId);
                return await BuyNFTSuiKiosk(nftId, nickname, secretKey, walletId);
            }
            string gaslessTx = await BuildBuySuiKioskTransaction(token, nftId, ToolExtensions.FindHighestNumber(coinRoot.Result.Data), kioskRoot.Result.Data[0].Data.Content.fields.Cap.Fields.For, kioskRoot.Result.Data[0].Data.ObjectId);
            ResultDev rootDev = await DevInspectTransactionBlock(walletId, gaslessTx);

            string response = await ExecuteGaslessTransactionBlock(nickname, sessionToken, gaslessTx, rootDev.Effects.GasUsed.ComputationCost + rootDev.Effects.GasUsed.StorageCost);

            return ConvertStringToStatus(response);
        }

        /// <summary>
        /// Unlist NFT.
        /// </summary>
        /// <param name="nftId">NFT id.</param>
        /// <param name="nickname">User nickname.</param>
        /// <param name="secretKey">Used to encrypt a user's wallet private key. Typically, this would tie to your app's registration and authentication.</param>
        /// <param name="walletId">The wallet address.</param>
        public static async Task<StatusTransaction> UnlistAsset(string nftId, string nickname, string secretKey, string walletId)
        {
            string sessionToken = await OnCreateSession(secretKey);
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = await SignPersonalMessage(nickname, timestamp, sessionToken);
            string token = await LoginToKeepsake(signature, timestamp);
            string gaslessTx = await UnlistAssetBuild(nftId, token);
            ResultDev rootDev = await DevInspectTransactionBlock(walletId, gaslessTx);
            string response = await ExecuteGaslessTransactionBlock(nickname, sessionToken, gaslessTx, rootDev.Effects.GasUsed.ComputationCost + rootDev.Effects.GasUsed.StorageCost);

            return ConvertStringToStatus(response);
        }

        /// <summary>
        /// Create kiosk.
        /// </summary>
        /// <param name="nickname">User nickname.</param>
        /// <param name="secretKey">Used to encrypt a user's wallet private key. Typically, this would tie to your app's registration and authentication.</param>
        /// <param name="walletId">The wallet address.</param>
        public static async Task<StatusTransaction> CreateKiosk(string nickname, string secretKey, string walletId)
        {
            string sessionToken = await OnCreateSession(secretKey);
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = await SignPersonalMessage(nickname, timestamp, sessionToken);
            string token = await LoginToKeepsake(signature, timestamp);
            string gaslessTx = await MakeObKiosk(token);
            ResultDev rootDev = await DevInspectTransactionBlock(walletId, gaslessTx);
            string response = await ExecuteGaslessTransactionBlock(nickname, sessionToken, gaslessTx, rootDev.Effects.GasUsed.ComputationCost + rootDev.Effects.GasUsed.StorageCost);

            return ConvertStringToStatus(response);
        }

        /// <summary>
        /// Create Sui kiosk.
        /// </summary>
        /// <param name="nickname">User nickname.</param>
        /// <param name="secretKey">Used to encrypt a user's wallet private key. Typically, this would tie to your app's registration and authentication.</param>
        /// <param name="walletId">The wallet address.</param>
        public static async Task<StatusTransaction> CreateSuiKiosk(string nickname, string secretKey, string walletId)
        {
            string sessionToken = await OnCreateSession(secretKey);
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = await SignPersonalMessage(nickname, timestamp, sessionToken);
            string token = await LoginToKeepsake(signature, timestamp);
            string gaslessTx = await MakeSuiKiosk(token);
            ResultDev rootDev = await DevInspectTransactionBlock(walletId, gaslessTx);
            string response = await ExecuteGaslessTransactionBlock(nickname, sessionToken, gaslessTx, rootDev.Effects.GasUsed.ComputationCost + rootDev.Effects.GasUsed.StorageCost);

            return ConvertStringToStatus(response);
        }

        /// <summary>
        /// Sell NFT.
        /// </summary>
        /// <param name="nftId">NFT id.</param>
        /// <param name="amount">NFT price.</param>
        /// <param name="nickname">User nickname.</param>
        /// <param name="secretKey">Used to encrypt a user's wallet private key. Typically, this would tie to your app's registration and authentication.</param>
        /// <param name="walletId">The wallet address.</param>
        public static async Task<StatusTransaction> SellNFT(string nftId, double amount, string nickname, string secretKey, string walletId)
        {
            string sessionToken = await OnCreateSession(secretKey);
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = await SignPersonalMessage(nickname, timestamp, sessionToken);
            string token = await LoginToKeepsake(signature, timestamp);
            KioskRootOwned kioskRoot = await GetOwnedObjectKiosk(walletId);
            if (kioskRoot.Result.Data.Count <= 0)
            {
                await CreateKiosk(nickname, secretKey, walletId);
                return await SellNFT(nftId, amount, nickname, secretKey, walletId);
            }
            string gaslessTx = await BuildSellTransaction(token, nftId, (amount * 1000000000).ToString(), kioskRoot.Result.Data[0].Data.Display.Data.Kiosk);
            ResultDev rootDev = await DevInspectTransactionBlock(walletId, gaslessTx);
            string response = await ExecuteGaslessTransactionBlock(nickname, sessionToken, gaslessTx, rootDev.Effects.GasUsed.ComputationCost + rootDev.Effects.GasUsed.StorageCost);

            return ConvertStringToStatus(response);
        }

        /// <summary>
        /// Sell NFT Sui Kiosk.
        /// </summary>
        /// <param name="nftId">NFT id.</param>
        /// <param name="amount">NFT price.</param>
        /// <param name="nickname">User nickname.</param>
        /// <param name="secretKey">Used to encrypt a user's wallet private key. Typically, this would tie to your app's registration and authentication.</param>
        /// <param name="walletId">The wallet address.</param>
        public static async Task<StatusTransaction> SellNFTSuiKiosk(string nftId, double amount, string nickname, string secretKey, string walletId)
        {
            string sessionToken = await OnCreateSession(secretKey);
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = await SignPersonalMessage(nickname, timestamp, sessionToken);
            string token = await LoginToKeepsake(signature, timestamp);
            KioskRootOwned kioskRoot = await GetOwnedObjectSuiKiosk(walletId);
            if (kioskRoot.Result.Data.Count <= 0)
            {
                await CreateSuiKiosk(nickname, secretKey, walletId);
                return await SellNFT(nftId, amount, nickname, secretKey, walletId);
            }

            Debug.Log("TX: token " + token);
            Debug.Log("TX: kiosk " + kioskRoot.Result.Data[0].Data.Content.fields.Cap.Fields.For);
            Debug.Log("TX: kiosk cap " + kioskRoot.Result.Data[0].Data.ObjectId);


            string gaslessTx = await BuildSellSuiKioskTransaction(token, nftId, (amount * 1000000000).ToString(), kioskRoot.Result.Data[0].Data.Content.fields.Cap.Fields.For, kioskRoot.Result.Data[0].Data.ObjectId);
            Debug.Log("STEPPING INTO: " + gaslessTx);
            ResultDev rootDev = await DevInspectTransactionBlock(walletId, gaslessTx);
            string response = await ExecuteGaslessTransactionBlock(nickname, sessionToken, gaslessTx, rootDev.Effects.GasUsed.ComputationCost + rootDev.Effects.GasUsed.StorageCost);

            return ConvertStringToStatus(response);
        }

        /// <summary>
        /// Withdrawing from Sui Kiosk
        /// </summary>
        /// <param name="nickname">user nickname</param>
        /// <param name="secretKey">Used to encrypt a user's wallet private key. Typically, this would tie to your app's registration and authentication.</param>
        /// <param name="walletId">The wallet address.</param>
        /// <param name="kiosk">Personal Sui kiosk address of user</param>
        /// <returns></returns>
       public static async Task<StatusTransaction> ClaimSuiKioskFunds(string nickname, string secretKey, string walletId, string kiosk)
       {
            //first check to ensure that there is profit in the account
            RootGetObject rootObject = await GetObject(kiosk);
            double profitTotal = Double.Parse(rootObject.Result.Data.Content.Fields.Profits);

            
            if(profitTotal >= 1)
            {
                string sessionToken = await OnCreateSession(secretKey);
                string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
                string signature = await SignPersonalMessage(nickname, timestamp, sessionToken);
                string token = await LoginToKeepsake(signature, timestamp);
                KioskRootOwned kioskRoot = await GetOwnedObjectSuiKiosk(walletId);
                if (kioskRoot.Result.Data.Count <= 0)
                {
                    await CreateSuiKiosk(nickname, secretKey, walletId);
                }
                
                string gaslessTx = await BuildWithdrawalSuiKioskProceeds(token, kioskRoot.Result.Data[0].Data.Content.fields.Cap.Fields.For, kioskRoot.Result.Data[0].Data.ObjectId);
                ResultDev rootDev = await DevInspectTransactionBlock(walletId, gaslessTx);
                string response = await ExecuteGaslessTransactionBlock(nickname, sessionToken, gaslessTx, rootDev.Effects.GasUsed.ComputationCost + rootDev.Effects.GasUsed.StorageCost);

                return ConvertStringToStatus(response);
            }else{
                
                return StatusTransaction.Failure;
            }


            
       }

       /// <summary>
        /// Claim Free NFT Asset from Gab Bag
        /// </summary>
        /// <param name="nickname">user nickname</param>
        /// <param name="secretKey">Used to encrypt a user's wallet private key. Typically, this would tie to your app's registration and authentication.</param>
        /// <param name="walletId">The wallet address.</param>
        /// <param name="kiosk">Personal Sui kiosk address of user</param>
        /// <param name="kioskCap">Personal Sui kiosk capability address of user</param>
        /// <param name="nftId">(optional) NFT ID of selected asset. If left blank, then a random asset will be assigned</param>
        /// <param name="bagId">Grab Bad ID of kiosk associated with the collection</param>
        /// <returns></returns>
       public static async Task<StatusTransaction> ClaimGiftShopNFTAsset(string nickname, string secretKey, string walletId, string kiosk, string kioskCap, string nftId, string bagId)
       {
            string sessionToken = await OnCreateSession(secretKey);
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = await SignPersonalMessage(nickname, timestamp, sessionToken);
            string token = await LoginToKeepsake(signature, timestamp);

            string gaslessTx = await BuildGiftShopTransaction(token, kiosk, kioskCap, nftId, bagId);
            ResultDev rootDev = await DevInspectTransactionBlock(walletId, gaslessTx);

            string response = await ExecuteGaslessTransactionBlock(nickname, sessionToken, gaslessTx, rootDev.Effects.GasUsed.ComputationCost + rootDev.Effects.GasUsed.StorageCost);

            return ConvertStringToStatus(response);
       }

        private static StatusTransaction ConvertStringToStatus(string response)
        {
            switch (response)
            {
                case "success":
                    return StatusTransaction.Success;
                default:
                    return StatusTransaction.Failure;
            }
        }
    }
}