using UnityEngine;
using UnityEngine.UI;

namespace MarketplaceSDK.Example.Game.UI
{
    public class LoginItem : MonoBehaviour
    {
        public Button LoginBtn;
        public Button SignUpBtn;
        public InputField NicknameField;
        public InputField SecretKeyField;
        public Text ErrorText;

        public void ClearItem()
        {
            NicknameField.text = "";
            SecretKeyField.text = "";
            ErrorText.text = "";
            ErrorText.gameObject.SetActive(false);
        }
    }
}