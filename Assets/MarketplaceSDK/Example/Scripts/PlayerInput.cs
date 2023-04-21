using System;
using UnityEngine;

namespace MarketplaceSDK.Example.Game.Input
{
    public class PlayerInput : MonoBehaviour
    {
        public Action onFire;

        private void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                onFire?.Invoke();
            }
        }
    }
}
