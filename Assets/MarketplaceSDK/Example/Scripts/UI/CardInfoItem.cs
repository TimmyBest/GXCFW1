using UnityEngine;
using UnityEngine.UI;

namespace MarketplaceSDK.Example.Game.UI
{
    public class CardInfoItem : MonoBehaviour
    {
        [HideInInspector] public int index;

        public Text NameText;
        public Text DescriptionText;
        public Text SpeedText;
        public Text ScaleText;
        public Text SideColorText;
        public Text EdgeColorText;
        public Text buyBtnText;
        public Button buyBtn;
    }
}
