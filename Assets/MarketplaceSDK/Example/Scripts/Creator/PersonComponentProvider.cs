using MarketplaceSDK.Example.Interfaces;
using UnityEngine;

namespace MarketplaceSDK.Example.Game.Provider
{
    public class PersonComponentProvider : MonoBehaviour, IPersonComponentProvider
    {
        [SerializeField] protected MeshRenderer _meshRenderer;

        public MeshRenderer GetMeshRenderer()
        {
            return _meshRenderer;
        }
    }
}