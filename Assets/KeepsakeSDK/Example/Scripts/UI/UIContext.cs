using KeepsakeSDK.Core.Enums;
using KeepsakeSDK.Example.Game.Creator;
using KeepsakeSDK.Example.Game.Enum;
using KeepsakeSDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace KeepsakeSDK.Example.Game.UI
{
    public class UIContext : MonoBehaviour
    {
        // public string _secretKey = "anewsecretkey";
        public string _secretKey = "GAME_KEY_1";
        [Space(25f)]
        [SerializeField] private Transform _contentMarket;
        [SerializeField] private Transform _contentMyNFT;
        [SerializeField] private Transform _contentGiftShop;
        [SerializeField] private GameObject _cardInfo;
        [SerializeField] private GameObject _mainMenuWindow;
        [SerializeField] private GameObject _loginWindow;
        [SerializeField] private GameObject _infoWindow;

        [SerializeField] private LoginItem _loginItem;
        [SerializeField] private MainMenuItem _mainMenuItem;
        public ActivityIndicatorItem ActivityIndicatorItem;
        [SerializeField] private MarketMenuItem _marketMenuItem;
        [SerializeField] private MyNFTMenuItem _myNftMenuItem;
        [SerializeField] private GiftShopMenuItem _giftShopMenuItem;

        private string _nickname = "";
        private string _walletId = "";

        // need to delete
        [SerializeField] private GameObject prefabCubix;
        [SerializeField] private GameContext _gameContext;

        private void Awake()
        {
            _mainMenuItem.MarketBtn.onClick.AddListener(async delegate {
                await UpdateWindows();

                _mainMenuWindow.SetActive(false);
                _marketMenuItem.gameObject.SetActive(true);
            });

            _mainMenuItem.MyNftBtn.onClick.AddListener(async delegate {
                await UpdateWindows();

                _mainMenuWindow.SetActive(false);
                _myNftMenuItem.gameObject.SetActive(true);
            });

            _mainMenuItem.GiftShopBtn.onClick.AddListener(async delegate {
                await UpdateWindows();

                _mainMenuWindow.SetActive(false);
                _giftShopMenuItem.gameObject.SetActive(true);
                Debug.Log("inside giftshop");
            });

            _marketMenuItem.backBtn.onClick.AddListener(delegate
            {

                _marketMenuItem.gameObject.SetActive(false);
                _mainMenuWindow.SetActive(true);
            });

            _myNftMenuItem.backBtn.onClick.AddListener(delegate
            {
                _myNftMenuItem.gameObject.SetActive(false);
                _mainMenuWindow.SetActive(true);
            });

            _giftShopMenuItem.backBtn.onClick.AddListener(delegate
            {
                _giftShopMenuItem.gameObject.SetActive(false);
                _mainMenuWindow.SetActive(true);
            });

            _mainMenuItem.UnloginBtn.onClick.AddListener(delegate
            {
                _loginItem.ClearItem();
                _mainMenuWindow.SetActive(false);
                _loginWindow.SetActive(true);
            });

            _mainMenuItem.RefreshBtn.onClick.AddListener(async delegate
            {
                ActivityIndicatorItem.Open();

                await OpenMainMenu(_nickname, _walletId);

                ActivityIndicatorItem.Close();
            });
        }

        public void InitializeUser(string nickname, string walletId)
        {
            _nickname = nickname;
            _walletId = walletId;
        }

        public async Task UpdateWindows()
        {
            ActivityIndicatorItem.Open();

            string response = await KeepsakeSDK.OnSearchListing(1, "650352d76fd2f3e0ba574ee4", "Cubies");
            Root root = JsonConvert.DeserializeObject<Root>(response);

            //Get the id and kiosk id of the gift shop bag to be used. Items in here a totally free
            string rootGiftShopId = root.Results[0].Collection.GrabBags[0].Id;
            string rootGiftShopKioskId = root.Results[0].Collection.GrabBags[0].KioskId;
            
            KioskRootOwned kioskRoot = await KeepsakeSDK.GetOwnedObjectSuiKiosk(_walletId);

            if(kioskRoot.Result.Data.Count == 0){
                await KeepsakeSDK.CreateSuiKiosk(_loginItem.NicknameField.text, _secretKey, _walletId);
                await UpdateWindows();
            }
            
            RootDynamic rootDynamic = await KeepsakeSDK.GetDynamicField(kioskRoot.Result.Data[0].Data.Content.fields.Cap.Fields.For);
            //Since the assets are in a gift shop kiosk, you can retrieve them the same way you do any dynamic field objects
            RootDynamic rootDynamicGift = await KeepsakeSDK.GetDynamicField(rootGiftShopKioskId);
            
            
            RootObjectType rootObjectType = await KeepsakeSDK.GetObjectType("650352d76fd2f3e0ba574ee4");
            List<string> objects = new List<string>();
            List<string> giftObjects = new List<string>();

            // We use rootObjectType to get the type of our NFTs and then compare if the type of the object in the kiosk is rootObjectType.Collection.FullType
            foreach (DataDynamic answer in rootDynamic.Result.Data)
            {
                if (answer.ObjectType == rootObjectType.Collection.FullType)
                {
                    objects.Add(answer.ObjectId);
                }
            }

            //Similar to the above, you want to check to make sure that the object types in the gift shop match the type, since anything can be placed in there
            foreach (DataDynamic solution in rootDynamicGift.Result.Data)
            {
                if (solution.ObjectType == rootObjectType.Collection.FullType)
                {
                    giftObjects.Add(solution.ObjectId);
                }
            }

            RootMulti rootNft = await KeepsakeSDK.GetMultiObjects(objects.ToArray());
            //now you can get all the objects info to display. Can be mixed in with marketplace items or have its own section
            RootMulti rootNftGift = await KeepsakeSDK.GetMultiObjects(giftObjects.ToArray());

            string sessionToken = await KeepsakeSDK.OnCreateSession(_secretKey);
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = await KeepsakeSDK.SignPersonalMessage(_nickname, timestamp, sessionToken);
            string token = await KeepsakeSDK.LoginToKeepsake(signature, timestamp);
            Root rootMyListing = await KeepsakeSDK.GetMyListing(token);

            /// This can be placed anywhere, but it checks to see if the users personal kiosk is greater than or equal to 1, if so then run the transaction to withdraw
            /// to the users wallet
            StatusTransaction status = await KeepsakeSDK.ClaimSuiKioskFunds(_nickname, _secretKey, _walletId, kioskRoot.Result.Data[0].Data.Content.fields.Cap.Fields.For);


            OpenMyNFT(rootNft.Result, rootMyListing.Results, false);
            OpenMarket(root.Results, kioskRoot.Result.Data[0].Data.Content.fields.Cap.Fields.For, false);
            OpenGiftShop(rootNftGift.Result, token, kioskRoot.Result.Data[0].Data.Content.fields.Cap.Fields.For, kioskRoot.Result.Data[0].Data.ObjectId, rootGiftShopId, false);

            ActivityIndicatorItem.Close();
        }

        public void OpenMarket(List<Result> results, string kiosk, bool open = true)
        {
            _marketMenuItem.gameObject.SetActive(open);

            for (int i = 0; i < _contentMarket.childCount; i++)
            {
                Transform childTransform = _contentMarket.GetChild(i);
                Destroy(childTransform.gameObject);
            }

            CharacterCreator CharacterCreator = new CharacterCreator();

            for (int i = 0; i < results.Count; i++)
            {
                CardInfoItem cardInfo = GameObject.Instantiate(_cardInfo, _contentMarket).GetComponent<CardInfoItem>();

                GameObject objectModel = CharacterCreator.CreateCharacter(prefabCubix, new Vector3(0, -10000, 0), "Cubix",
                    (int)ColorType.Parse(typeof(ColorType), results[i].Nft.Fields.SideColor),
                    (int)ColorType.Parse(typeof(ColorType), results[i].Nft.Fields.EdgeColor));

                SnapshotCamera snapshotCamera = SnapshotCamera.MakeSnapshotCamera(0);
                Texture2D snapshot = snapshotCamera.TakeObjectSnapshot(objectModel);
                cardInfo.CubeImage.sprite = Sprite.Create(snapshot, new Rect(0, 0, 128, 128), new Vector2());
                Destroy(objectModel);
                Destroy(snapshotCamera);

                cardInfo.NameText.text = results[i].Nft.Name;
                cardInfo.PriceText.text = (results[i].SalePrice / 1000000000).ToString() + " SUI";
                cardInfo.SpeedText.text = "Speed: " + results[i].Nft.Fields.Speed.ToString();
                cardInfo.ScaleText.text = "Size: " + results[i].Nft.Fields.Size.ToString();
                cardInfo.SideColorText.text = "Side Color: " + results[i].Nft.Fields.SideColor;
                cardInfo.EdgeColorText.text = "Edge Color: " + results[i].Nft.Fields.EdgeColor;

                cardInfo.index = i;

                if (kiosk == results[i].SellerKiosk) 
                {
                    cardInfo.BuyBtnText.text = "Unlist";
                    cardInfo.BuyBtn.onClick.AddListener(async delegate {
                        ActivityIndicatorItem.Open();

                        StatusTransaction status = await KeepsakeSDK.UnlistAsset(results[cardInfo.index].Id, _nickname, _secretKey, _walletId);
                        string tooltip = status.ToString() + " transaction";
                        await OpenMainMenu(_nickname, _walletId, tooltip);

                        ActivityIndicatorItem.Close();
                        _marketMenuItem.gameObject.SetActive(false);
                    });
                }
                else
                {
                    cardInfo.BuyBtnText.text = "Buy";
                    cardInfo.BuyBtn.onClick.AddListener(async delegate {
                        ActivityIndicatorItem.Open();

                        StatusTransaction status = await KeepsakeSDK.BuyNFTSuiKiosk(results[cardInfo.index].Id, _nickname, _secretKey, _walletId);
                        string tooltip = status.ToString() + " transaction";
                        await OpenMainMenu(_nickname, _walletId, tooltip);

                        ActivityIndicatorItem.Close();
                        _marketMenuItem.gameObject.SetActive(false);
                    });
                }
            }
        }

        public void OpenInfoWindow()
        {
            _infoWindow.SetActive(true);
        }

        public void OpenMyNFT(List<ResultMulti> results, List<Result> resultListing, bool open = true)
        {
            
            _myNftMenuItem.gameObject.SetActive(open);

            for (int i = 0; i < _contentMyNFT.childCount; i++)
            {
                Transform childTransform = _contentMyNFT.GetChild(i);
                Destroy(childTransform.gameObject);
            }
            CharacterCreator CharacterCreator = new CharacterCreator();

            for (int i = 0; i < results.Count; i++)
            {
                CardInfoItem cardInfo = GameObject.Instantiate(_cardInfo, _contentMyNFT).GetComponent<CardInfoItem>();


                float speed = 0.0f;
                float size = 0.0f;
                string edgeColor = "";
                string sideColor = "";

                for (int k = 0; k < results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents.Count; k++)
                {

                    switch (results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Key)
                    {
                        case "Size":
                            Debug.Log("Size: " + results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value);
                            size = float.Parse(results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value, CultureInfo.InvariantCulture.NumberFormat);

                            break;
                        case "Speed":
                            Debug.Log("Speed: " + results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value);
                            speed = float.Parse(results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value, CultureInfo.InvariantCulture.NumberFormat);

                            break;
                        case "Edge Color":
                            Debug.Log("Edge Color: " + results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value);
                            edgeColor = results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value;

                            break;
                        case "Side Color":
                            Debug.Log("Side Color: " + results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value);
                            sideColor = results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value;

                            break;
                        default:
                            break;
                    }

                }

                GameObject objectModel =  CharacterCreator.CreateCharacter(prefabCubix, new Vector3(0, -10000, 0), "Cubix",
                    (int)ColorType.Parse(typeof(ColorType), sideColor),
                    (int)ColorType.Parse(typeof(ColorType), edgeColor));

                SnapshotCamera snapshotCamera = SnapshotCamera.MakeSnapshotCamera(0);
                Texture2D snapshot = snapshotCamera.TakeObjectSnapshot(objectModel);

                Destroy(objectModel);
                Destroy(snapshotCamera);

                cardInfo.CubeImage.sprite = Sprite.Create(snapshot, new Rect(0, 0, 128, 128), new Vector2());
                cardInfo.NameText.text = results[i].Data.Content.Fields.Name;
                cardInfo.SpeedText.text = "Speed: " + speed.ToString();
                cardInfo.ScaleText.text = "Size: " + size.ToString();
                cardInfo.SideColorText.text = "Side Color: " + sideColor;
                cardInfo.EdgeColorText.text = "Edge Color: " + edgeColor;
                cardInfo.BuyBtnText.text = "Select";
                cardInfo.SellBtnText.text = "Sell";
                cardInfo.PriceText.text = "Owned";

                cardInfo.Id = results[i].Data.Content.Fields.Id.Id;
                bool listed = false;
                string listingId = "";
                foreach(Result res in resultListing)
                {
                    if (cardInfo.Id == res.Nft.ObjectId)
                    {
                        listingId = res.Id;
                        listed = true;
                    }
                }

                cardInfo.BuyBtn.onClick.AddListener(delegate {
                    _gameContext.InitGame(new Result(size, speed, edgeColor, sideColor));
                    _myNftMenuItem.gameObject.SetActive(false);
                });

                if (!listed)
                {
                    cardInfo.SellBtn.onClick.AddListener(delegate
                    {
                        cardInfo.SellingTooltip.SetActive(true);
                    });

                    cardInfo.ConfirmBtn.onClick.AddListener(async delegate
                    {
                        ActivityIndicatorItem.Open();

                        StatusTransaction status = await KeepsakeSDK.SellNFTSuiKiosk(cardInfo.Id, double.Parse(cardInfo.sellInputField.text, CultureInfo.InvariantCulture.NumberFormat), _nickname, _secretKey, _walletId);
                        string tooltip = status.ToString() + " transaction";
                        await OpenMainMenu(_nickname, _walletId, tooltip);

                        ActivityIndicatorItem.Close();
                        _myNftMenuItem.gameObject.SetActive(false);
                    });
                    cardInfo.CancelBtn.onClick.AddListener(delegate
                    {
                        cardInfo.SellingTooltip.SetActive(false);
                    });

                }
                else
                {
                    cardInfo.SellBtnText.text = "Unlist";

                    cardInfo.SellBtn.onClick.AddListener(async delegate
                    {
                        ActivityIndicatorItem.Open();

                        StatusTransaction status = await KeepsakeSDK.UnlistAsset(listingId, _nickname, _secretKey, _walletId);

                        string tooltip = status.ToString() + " transaction";
                        await OpenMainMenu(_nickname, _walletId, tooltip);

                        ActivityIndicatorItem.Close();
                        _myNftMenuItem.gameObject.SetActive(false);
                    });
                }

                cardInfo.SellBtn.gameObject.SetActive(true);
            }
        }

        public void OpenGiftShop(List<ResultMulti> results, string token, string kiosk, string kioskCap, string bagId, bool open = true)
        {
            
            _giftShopMenuItem.gameObject.SetActive(open);

            for (int i = 0; i < _contentGiftShop.childCount; i++)
            {
                Transform childTransform = _contentGiftShop.GetChild(i);
                Destroy(childTransform.gameObject);
            }
            CharacterCreator CharacterCreator = new CharacterCreator();

            for (int i = 0; i < results.Count; i++)
            {
                CardInfoItem cardInfo = GameObject.Instantiate(_cardInfo, _contentGiftShop).GetComponent<CardInfoItem>();

                float speed = 0.0f;
                float size = 0.0f;
                string edgeColor = "";
                string sideColor = "";

                for (int k = 0; k < results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents.Count; k++)
                {

                    switch (results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Key)
                    {
                        case "Size":
                            Debug.Log("Size: " + results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value);
                             size = float.Parse(results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value, CultureInfo.InvariantCulture.NumberFormat);

                            break;
                        case "Speed":
                            Debug.Log("Speed: " + results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value);
                             speed = float.Parse(results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value, CultureInfo.InvariantCulture.NumberFormat);

                            break;
                        case "Edge Color":
                            Debug.Log("Edge Color: " + results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value);
                             edgeColor = results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value;

                            break;
                        case "Side Color":
                            Debug.Log("Side Color: " + results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value);
                             sideColor = results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[k].Fields.Value;

                            break;
                        default:
                            break;
                    }

                }




                GameObject objectModel =  CharacterCreator.CreateCharacter(prefabCubix, new Vector3(0, -10000, 0), "Cubix",
                    (int)ColorType.Parse(typeof(ColorType), sideColor),
                    (int)ColorType.Parse(typeof(ColorType), edgeColor));

                SnapshotCamera snapshotCamera = SnapshotCamera.MakeSnapshotCamera(0);
                Texture2D snapshot = snapshotCamera.TakeObjectSnapshot(objectModel);

                Destroy(objectModel);
                Destroy(snapshotCamera);

                cardInfo.CubeImage.sprite = Sprite.Create(snapshot, new Rect(0, 0, 128, 128), new Vector2());
                cardInfo.NameText.text = results[i].Data.Content.Fields.Name;
                cardInfo.SpeedText.text = "Speed: " + speed.ToString();
                cardInfo.ScaleText.text = "Size: " + size.ToString();
                cardInfo.SideColorText.text = "Side Color: " + sideColor;
                cardInfo.EdgeColorText.text = "Edge Color: " + edgeColor;
                cardInfo.BuyBtnText.text = "Gift";
                cardInfo.SellBtnText.text = "Sell";
                cardInfo.PriceText.text = "Gift";

                cardInfo.Id = results[i].Data.Content.Fields.Id.Id;
                bool listed = false;
                

                if (!listed)
                {
                    cardInfo.BuyBtnText.text = "Acquire";
                    cardInfo.BuyBtn.onClick.AddListener(async delegate {
                        ActivityIndicatorItem.Open();

                        StatusTransaction status = await KeepsakeSDK.ClaimGiftShopNFTAsset(_nickname, _secretKey, _walletId, kiosk, kioskCap, cardInfo.Id, bagId);
                        string tooltip = status.ToString() + " transaction";
                        await OpenMainMenu(_nickname, _walletId, tooltip);
                        _giftShopMenuItem.gameObject.SetActive(false);
                        ActivityIndicatorItem.Close();
                    });  

                }

                cardInfo.SellBtn.gameObject.SetActive(false);
            }
        }

        public async Task OpenMainMenu(string nickname, string walletId, string tooltip = "")
        {
            RootBalance coinRoot = await KeepsakeSDK.GetWalletBalance(walletId);
            double balance = coinRoot.Result.TotalBalance / 1000000000;

            _mainMenuWindow.SetActive(true);
            _mainMenuItem.NicknameText.text = nickname;
            _mainMenuItem.WalletIdText.text = walletId;
            _mainMenuItem.BalanceText.text = balance.ToString() + " SUI";
            _mainMenuItem.PlayTooltip(tooltip);
        }

        public void OpenLoginWindow(string errorMessage = "")
        {
            _loginWindow.SetActive(true);

            _loginItem.ErrorText.gameObject.SetActive(errorMessage.Length > 0);
            _loginItem.ErrorText.text = errorMessage;

            _loginItem.LoginBtn.onClick.AddListener(async delegate {
                ActivityIndicatorItem.Open();

                StatusAuth result = await KeepsakeSDK.AuthorizationAPI(_loginItem.NicknameField.text, _secretKey);
                if(result == StatusAuth.Success)
                {
                    string walletId = await KeepsakeSDK.GetWallet(_loginItem.NicknameField.text);
                    InitializeUser(_loginItem.NicknameField.text, walletId);

                    await OpenMainMenu(_loginItem.NicknameField.text, walletId);
                }
                else
                {
                    Debug.Log("Error: " + ConvertToHumanReadable(result));
                    errorMessage = ConvertToHumanReadable(result);
                    _loginItem.ErrorText.gameObject.SetActive(errorMessage.Length > 0);
                    _loginItem.ErrorText.text = errorMessage;
                    ActivityIndicatorItem.Close();
                    return;
                }
                _loginWindow.SetActive(false);
                

                ActivityIndicatorItem.Close();
            });

            _loginItem.SignUpBtn.onClick.AddListener(async delegate {
                ActivityIndicatorItem.Open();

                StatusRegister result = await KeepsakeSDK.SignUpAccount(_loginItem.NicknameField.text, _secretKey);
                if (result == StatusRegister.Success)
                {
                    string walletId = await KeepsakeSDK.GetWallet(_loginItem.NicknameField.text);

                    await KeepsakeSDK.CreateSuiKiosk(_loginItem.NicknameField.text, _secretKey, walletId);

                    InitializeUser(_loginItem.NicknameField.text, walletId);

                    await OpenMainMenu(_loginItem.NicknameField.text, walletId);
                }
                else
                {
                    Debug.Log("Error: " + ConvertToHumanReadable(result));
                    errorMessage = ConvertToHumanReadable(result);
                    _loginItem.ErrorText.gameObject.SetActive(errorMessage.Length > 0);
                    _loginItem.ErrorText.text = errorMessage;
                    ActivityIndicatorItem.Close();
                    return;
                }

                _loginWindow.SetActive(false);

                ActivityIndicatorItem.Close();
            });
        }

        public string ConvertToHumanReadable(StatusAuth result)
        {
            switch (result)
            {
                case StatusAuth.WalletNotFound:
                    return "Wallet ID not found!";
                case StatusAuth.WrongSecretKey:
                    return "Wrong secret key!";
                default:
                    return "";
            }
        }

        public string ConvertToHumanReadable(StatusRegister result)
        {
            switch (result)
            {
                case StatusRegister.NicknameEmpty:
                    return "Name field is empty!";
                case StatusRegister.WalletExist:
                    return "Wallet ID already exist!";
                default:
                    return "";
            }
        }
    }
}
