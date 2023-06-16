using MarketplaceSDK.Models;
using System;
using System.Collections.Generic;
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

        [SerializeField] private LoginItem _loginItem;
        [SerializeField] private MainMenuItem _mainMenuItem;
        public ActivityIndicatorItem ActivityIndicatorItem;
        [SerializeField] private MarketMenuItem _marketMenuItem;
        [SerializeField] private MyNFTMenuItem _myNftMenuItem;


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

            for (int i = 0; i < results.Count; i++)
            {
                CardInfoItem cardInfo = GameObject.Instantiate(_cardInfo, _contentMarket).GetComponent<CardInfoItem>();

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

        public void OpenMyNFT(Action<string> action, List<Result> results, bool open = true)
        {
            _myNftMenuItem.gameObject.SetActive(open);

            for (int i = _contentMyNFT.childCount - 1; i >= 0; i--)
            {
                Transform childTransform = _contentMyNFT.GetChild(i);
                Destroy(childTransform.gameObject);
            }

            for (int i = 0; i < results.Count; i++)
            {
                CardInfoItem cardInfo = GameObject.Instantiate(_cardInfo, _contentMyNFT).GetComponent<CardInfoItem>();

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
