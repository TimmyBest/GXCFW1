using UnityEngine;

namespace KeepsakeSDK.Example.Game.UI
{
    public class ActivityIndicatorItem : MonoBehaviour
    {
        [SerializeField] private Transform _indicator;
        [SerializeField] private GameObject _panel;

        public void Update()
        {
            if(_indicator.gameObject.active)
                _indicator.Rotate(0f, 0f, 90f * Time.deltaTime, Space.Self);
        }

        public void Open() { _panel.SetActive(true); }

        public void Close() { _panel.SetActive(false); }
    }
}