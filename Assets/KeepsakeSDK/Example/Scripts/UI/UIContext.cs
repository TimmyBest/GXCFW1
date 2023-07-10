using KeepsakeSDK.Core.Enums;
using KeepsakeSDK.Example.Game.Creator;
using KeepsakeSDK.Example.Game.Enum;
using KeepsakeSDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;

namespace KeepsakeSDK.Example.Game.UI
{
    public class UIContext : MonoBehaviour
    {
        public string _secretKey = "anewsecretkey";

        [SerializeField] private Transform _contentMarket;
        [SerializeField] private Transform _contentMyNFT;
        [SerializeField] private GameObject _cardInfo;
        [SerializeField] private GameObject _mainMenuWindow;
        [SerializeField] private GameObject _loginWindow;
        [SerializeField] private GameObject _infoWindow;

        [SerializeField] private LoginItem _loginItem;
        [SerializeField] private MainMenuItem _mainMenuItem;
        public ActivityIndicatorItem ActivityIndicatorItem;
        [SerializeField] private MarketMenuItem _marketMenuItem;
        [SerializeField] private MyNFTMenuItem _myNftMenuItem;

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

            Root root = await KeepsakeSDK.OnSearchListing(1, "6462c8af23a2b24070683fd1", "Whacky Cube Smash");

            KioskRootOwned kioskRoot = await KeepsakeSDK.GetOwnedObjectKiosk(_walletId);
            RootDynamic rootDynamic = await KeepsakeSDK.GetDynamicField(kioskRoot.Result.Data[0].Data.Display.Data.Kiosk);
            RootObjectType rootObjectType = await KeepsakeSDK.GetObjectType("6462c8af23a2b24070683fd1");
            List<string> objects = new();

            foreach (DataDynamic answer in rootDynamic.Result.Data)
            {
                if (answer.ObjectType == rootObjectType.Collection.FullType)
                {
                    objects.Add(answer.ObjectId);
                }
            }
            RootMulti rootNft = await KeepsakeSDK.GetMultiObjects(objects.ToArray());
            string sessionToken = await KeepsakeSDK.OnCreateSession(_secretKey);
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string signature = await KeepsakeSDK.SignPersonalMessage(_nickname, timestamp, sessionToken);
            string token = await KeepsakeSDK.LoginToKeepsake(signature, timestamp);
            Root rootMyListing = await KeepsakeSDK.GetMyListing(token);

            OpenMyNFT(rootNft.Result, rootMyListing.Results, kioskRoot.Result.Data[0].Data.Display.Data.Kiosk, false);
            OpenMarket(root.Results, kioskRoot.Result.Data[0].Data.Display.Data.Kiosk, false);

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

            PersonCreator personCreator = new PersonCreator();

            for (int i = 0; i < results.Count; i++)
            {
                CardInfoItem cardInfo = GameObject.Instantiate(_cardInfo, _contentMarket).GetComponent<CardInfoItem>();

                GameObject objectModel = personCreator.CreatePerson(prefabCubix, new Vector3(0, -10000, 0), "Cubix",
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

                        await KeepsakeSDK.UnlistAssetBuild(results[cardInfo.index].Id, _nickname, _secretKey, _walletId);
                        await OpenMainMenu(_nickname, _walletId);

                        ActivityIndicatorItem.Close();
                        _marketMenuItem.gameObject.SetActive(false);
                    });
                }
                else
                {
                    cardInfo.BuyBtnText.text = "Buy";
                    cardInfo.BuyBtn.onClick.AddListener(async delegate {
                        ActivityIndicatorItem.Open();

                        await KeepsakeSDK.BuyNFT(results[cardInfo.index].Id, _nickname, _secretKey, _walletId);
                        await OpenMainMenu(_nickname, _walletId);

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

        public void OpenMyNFT(List<ResultMulti> results, List<Result> resultListing, string kiosk, bool open = true)
        {
            _myNftMenuItem.gameObject.SetActive(open);

            for (int i = 0; i < _contentMyNFT.childCount; i++)
            {
                Transform childTransform = _contentMyNFT.GetChild(i);
                Destroy(childTransform.gameObject);
            }
            PersonCreator personCreator = new PersonCreator();

            for (int i = 0; i < results.Count; i++)
            {
                CardInfoItem cardInfo = GameObject.Instantiate(_cardInfo, _contentMyNFT).GetComponent<CardInfoItem>();

                float speed = float.Parse(results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[1].Fields.Value, CultureInfo.InvariantCulture.NumberFormat);
                float size = float.Parse(results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[0].Fields.Value, CultureInfo.InvariantCulture.NumberFormat);
                string edgeColor = results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[2].Fields.Value;
                string sideColor = results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[3].Fields.Value;

                GameObject objectModel =  personCreator.CreatePerson(prefabCubix, new Vector3(0, -10000, 0), "Cubix",
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

                        await KeepsakeSDK.SellNFT(cardInfo.Id, double.Parse(cardInfo.sellInputField.text, CultureInfo.InvariantCulture.NumberFormat), _nickname, _secretKey, _walletId);
                        await OpenMainMenu(_nickname, _walletId);

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

                        await KeepsakeSDK.UnlistAssetBuild(listingId, _nickname, _secretKey, _walletId);
                        await OpenMainMenu(_nickname, _walletId);

                        ActivityIndicatorItem.Close();
                        _myNftMenuItem.gameObject.SetActive(false);
                    });
                }

                cardInfo.SellBtn.gameObject.SetActive(true);
            }
        }

        public async Task OpenMainMenu(string nickname, string walletId)
        {
            RootBalance coinRoot = await KeepsakeSDK.GetWalletBalance(walletId);
            double balance = coinRoot.Result.TotalBalance / 1000000000;

            _mainMenuWindow.SetActive(true);
            _mainMenuItem.NicknameText.text = nickname;
            _mainMenuItem.WalletIdText.text = walletId;
            _mainMenuItem.BalanceText.text = balance.ToString() + " SUI";
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
                    OpenLoginWindow(ConvertToHumanReadable(result));
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

                    await KeepsakeSDK.CreateKiosk(_loginItem.NicknameField.text, _secretKey, walletId);

                    InitializeUser(_loginItem.NicknameField.text, walletId);

                    await OpenMainMenu(_loginItem.NicknameField.text, walletId);
                }
                else
                {
                    OpenLoginWindow(ConvertToHumanReadable(result));
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
