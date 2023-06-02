using MarketplaceSDK.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarketplaceSDK.Example.Game.UI
{
    public class UIContext : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private GameObject _cardInfo;
        [SerializeField] private GameObject _marketWindow;
        [SerializeField] private GameObject _mainMenuWindow;

        public void OpenMarket(Action<Result> action, List<Result> results)
        {
            _marketWindow.SetActive(true);

            for (int i = 0; i < results.Count; i++)
            {
                CardInfoItem cardInfo = GameObject.Instantiate(_cardInfo, _content).GetComponent<CardInfoItem>();

                cardInfo.NameText.text = results[i].Nft.Name;
                cardInfo.DescriptionText.text = results[i].Nft.Description;
                cardInfo.SpeedText.text = "Speed: " + results[i].Nft.Fields.Speed.ToString();
                cardInfo.ScaleText.text = "Size: " + results[i].Nft.Fields.Size.ToString();
                cardInfo.SideColorText.text = "Side Color: " + results[i].Nft.Fields.SideColor;
                cardInfo.EdgeColorText.text = "Edge Color: " + results[i].Nft.Fields.EdgeColor;
                cardInfo.buyBtnText.text = "Buy";

                cardInfo.index = i;

                cardInfo.buyBtn.onClick.AddListener(delegate {
                    action?.Invoke(results[cardInfo.index]);
                    _marketWindow.SetActive(false);
                    _mainMenuWindow.SetActive(true);
                });
            }
        }

        public void OpenMainMenu()
        {
            _mainMenuWindow.SetActive(true);
        }
    }
}
