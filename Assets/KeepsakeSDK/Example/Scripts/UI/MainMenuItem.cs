using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KeepsakeSDK.Example.Game.UI
{
    public class MainMenuItem : MonoBehaviour
    {
        public Text NicknameText;
        public Text WalletIdText;
        public Text BalanceText;
        public Text TooltipText;

        public Button MyNftBtn;
        public Button MarketBtn;
        public Button UnloginBtn;
        public Button RefreshBtn;

        public void PlayTooltip(string tooltipText)
        {
            StartCoroutine(PlayTooltipCoroutine(tooltipText));
        }

        private IEnumerator PlayTooltipCoroutine(string tooltipText)
        {
            TooltipText.text = tooltipText;
            yield return new WaitForSeconds(3);
            TooltipText.text = "";
        }
    }
}