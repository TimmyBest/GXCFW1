using KeepsakeSDK.Example.Interfaces;
using UnityEngine;

namespace KeepsakeSDK.Example.Game.Provider
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