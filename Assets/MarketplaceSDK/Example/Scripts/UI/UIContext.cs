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


        private void Awake()
        {
            _mainMenuItem.MarketBtn.onClick.AddListener(delegate {
                _mainMenuWindow.SetActive(false);
                _marketMenuItem.gameObject.SetActive(true);
            });

            _mainMenuItem.MyNftBtn.onClick.AddListener(delegate {
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
        }

        public void OpenMarket(Action<string> action, List<Result> results, bool open = true)
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
                cardInfo.buyBtnText.text = "Buy";

                cardInfo.index = i;

                cardInfo.buyBtn.onClick.AddListener(delegate {
                    action?.Invoke(results[cardInfo.index].Id);
                });
            }
        }

        public void OpenInfoWindow()
        {
            _infoWindow.SetActive(true);
        }

        public void OpenMyNFT(Action<Result> actionGame, Action<string> sellNft, List<ResultMulti> results, bool open = true)
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
                string edgeColor = results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[3].Fields.Value;
                string sideColor = results[i].Data.Content.Fields.Attributes.Fields.map.Fields.Contents[2].Fields.Value;

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
                cardInfo.SideColorText.text = "Side Color: " + edgeColor;
                cardInfo.EdgeColorText.text = "Edge Color: " + sideColor;
                cardInfo.buyBtnText.text = "Select";
                cardInfo.sellBtnText.text = "Sell";
                cardInfo.PriceText.text = "Owned";

                cardInfo.Id = results[i].Data.Content.Fields.Id.Id;

                cardInfo.buyBtn.onClick.AddListener(delegate {
                    actionGame?.Invoke(new Result(size, speed, edgeColor, sideColor));
                    _myNftMenuItem.gameObject.SetActive(false);
                });
                cardInfo.sellBtn.onClick.AddListener(delegate
                {
                    sellNft?.Invoke(cardInfo.Id);
                    _myNftMenuItem.gameObject.SetActive(false);
                });

                cardInfo.sellBtn.gameObject.SetActive(true);
            }
        }

        public void OpenMainMenu(string nickname, string walletId, string balance)
        {
            _mainMenuWindow.SetActive(true);
            _mainMenuItem.NicknameText.text = nickname;
            _mainMenuItem.WalletIdText.text = walletId;
            _mainMenuItem.BalanceText.text = balance + " SUI";
        }

        public void OpenLoginWindow(Action<string> action)
        {
            _loginWindow.SetActive(true);

            _loginItem.LoginBtn.onClick.AddListener(delegate {
                action?.Invoke(_loginItem.NicknameField.text);
                _loginWindow.SetActive(false);
            });
        }
    }
}
