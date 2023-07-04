using MarketplaceSDK.Example.Game.Creator;
using MarketplaceSDK.Example.Game.Enum;
using MarketplaceSDK.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace MarketplaceSDK.Example.Game.UI
{
    public class UIContext : MonoBehaviour
    {
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


        // need to delete
        [SerializeField] private GameObject prefabCubix;

        [SerializeField] private GameContext _gameContext;

        private void Awake()
        {
            _mainMenuItem.MarketBtn.onClick.AddListener(async delegate {
                await _gameContext.UpdateWindows();

                _mainMenuWindow.SetActive(false);
                _marketMenuItem.gameObject.SetActive(true);
            });

            _mainMenuItem.MyNftBtn.onClick.AddListener(async delegate {
                await _gameContext.UpdateWindows();

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
        }

        public void OpenMarket(Action<string> action, Action<string> unlistAction, List<Result> results, string kiosk, bool open = true)
        {
            _marketMenuItem.gameObject.SetActive(open);

            for (int i = _contentMarket.childCount - 1; i >= 0; i--)
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
                    cardInfo.BuyBtn.onClick.AddListener(delegate {
                        unlistAction?.Invoke(results[cardInfo.index].Id);
                        _marketMenuItem.gameObject.SetActive(false);
                    });
                }
                else
                {
                    cardInfo.BuyBtnText.text = "Buy";
                    cardInfo.BuyBtn.onClick.AddListener(delegate {
                        action?.Invoke(results[cardInfo.index].Id);
                        _marketMenuItem.gameObject.SetActive(false);
                    });
                }
            }
        }

        public void OpenInfoWindow()
        {
            _infoWindow.SetActive(true);
        }

        public void OpenMyNFT(Action<Result> actionGame, Action<string, double> sellNft, Action<string> unlistAction, List<ResultMulti> results, List<Result> resultListing, string kiosk, bool open = true)
        {
            _myNftMenuItem.gameObject.SetActive(open);

            for (int i = _contentMyNFT.childCount - 1; i >= 0; i--)
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
                    actionGame?.Invoke(new Result(size, speed, edgeColor, sideColor));
                    _myNftMenuItem.gameObject.SetActive(false);
                });

                if (!listed)
                {
                    cardInfo.SellBtn.onClick.AddListener(delegate
                    {
                        cardInfo.SellingTooltip.SetActive(true);
                    });

                    cardInfo.ConfirmBtn.onClick.AddListener(delegate
                    {
                        sellNft?.Invoke(cardInfo.Id, double.Parse(cardInfo.sellInputField.text, CultureInfo.InvariantCulture.NumberFormat));
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

                    cardInfo.SellBtn.onClick.AddListener(delegate
                    {
                        unlistAction?.Invoke(listingId);
                        _myNftMenuItem.gameObject.SetActive(false);
                    });
                }

                cardInfo.SellBtn.gameObject.SetActive(true);
            }
        }

        public void OpenMainMenu(string nickname, string walletId, string balance)
        {
            _mainMenuWindow.SetActive(true);
            _mainMenuItem.NicknameText.text = nickname;
            _mainMenuItem.WalletIdText.text = walletId;
            _mainMenuItem.BalanceText.text = balance + " SUI";
        }

        public void OpenLoginWindow(Action<string, string> actionLogin, Action<string, string> actionRegistr, string errorMessage = "")
        {
            _loginWindow.SetActive(true);

            _loginItem.ErrorText.gameObject.SetActive(errorMessage.Length > 0);
            _loginItem.ErrorText.text = errorMessage;

            _loginItem.LoginBtn.onClick.AddListener(delegate {
                actionLogin?.Invoke(_loginItem.NicknameField.text, _loginItem.SecretKeyField.text);
                _loginWindow.SetActive(false);
            });

            _loginItem.SignUpBtn.onClick.AddListener(delegate {
                actionRegistr?.Invoke(_loginItem.NicknameField.text, _loginItem.SecretKeyField.text);
                _loginWindow.SetActive(false);
            });
        }
    }
}
