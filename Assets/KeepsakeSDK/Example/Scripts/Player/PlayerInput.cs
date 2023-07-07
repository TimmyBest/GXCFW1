using System;
using UnityEngine;

namespace KeepsakeSDK.Example.Game.Input
{
    public class PlayerInput : MonoBehaviour
    {
        public Action onFire;
        public Action onHold;

        private void Update()
        {
            if (UnityEngine.Input.GetMouseButton(0))
            {
                onHold?.Invoke();
            }
            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                onFire?.Invoke();
            }
        }
    }
}
