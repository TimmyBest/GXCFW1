using UnityEngine;
using UnityEngine.UI;

namespace MarketplaceSDK.Example.Game.UI
{
    public class CardInfoItem : MonoBehaviour
    {
        [HideInInspector] public int index;
        [HideInInspector] public string Id;

        public Image CubeImage;
        public Text NameText;
        public Text PriceText;
        public Text SpeedText;
        public Text ScaleText;
        public Text SideColorText;
        public Text EdgeColorText;
        public Text buyBtnText;
        public Text sellBtnText;
        public Button buyBtn;
        public Button sellBtn;
    }
}
