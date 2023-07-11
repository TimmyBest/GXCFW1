using KeepsakeSDK.Example.Interfaces;
using UnityEngine;

namespace KeepsakeSDK.Example.Game.Provider
{
    public class CharacterComponentProvider : MonoBehaviour, ICharacterComponentProvider
    {
        [SerializeField] protected MeshRenderer _meshRenderer;

        public MeshRenderer GetMeshRenderer()
        {
            return _meshRenderer;
        }
    }
}