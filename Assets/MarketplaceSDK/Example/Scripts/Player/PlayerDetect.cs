using System;
using UnityEngine;

namespace MarketplaceSDK.Example.Game.Player.Detect
{
    public class PlayerDetect : MonoBehaviour
    {
        public Action<Collider> action;

        public void OnTriggerEnter(Collider other)
        {
            action?.Invoke(other);
        }
    }
}
