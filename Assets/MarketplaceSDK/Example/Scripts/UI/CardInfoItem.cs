using UnityEngine;
using UnityEngine.UI;

namespace MarketplaceSDK.Example.Game.UI
{
    public class CardInfoItem : MonoBehaviour
    {
        [HideInInspector] public int index;
        [HideInInspector] public string Id;

        public GameObject SellingTooltip;
        public Image CubeImage;
        public InputField sellInputField;
        public Text NameText;
        public Text PriceText;
        public Text SpeedText;
        public Text ScaleText;
        public Text SideColorText;
        public Text EdgeColorText;
        public Text BuyBtnText;
        public Text SellBtnText;
        public Text ConfirmBtnText;
        public Text CancelBtnText;
        public Button BuyBtn;
        public Button SellBtn;
        public Button ConfirmBtn;
        public Button CancelBtn;
    }
}
