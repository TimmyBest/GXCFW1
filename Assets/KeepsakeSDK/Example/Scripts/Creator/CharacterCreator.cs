using KeepsakeSDK.Example.Game.Provider;
using KeepsakeSDK.Example.Interfaces;
using System;
using UnityEditor;
using UnityEngine;

namespace KeepsakeSDK.Example.Game.Creator
{
    public class CharacterCreator : ICharacterCreator
    {
        private const string MATERIAL_PATH = "KeepsakeSDK/Materials/";

        public GameObject CreateCharacter(GameObject prefab, Vector3 position, string type, int sideColor, int edgeColor)
        {
            GameObject clone = GameObject.Instantiate(prefab, position, prefab.transform.rotation);

            Material sideMaterial = Resources.Load<Material>(MATERIAL_PATH + type + $"/{type}_Col2 {sideColor}");
            Material frontMaterial = Resources.Load<Material>(MATERIAL_PATH + type + $"/{type}_Col1 {edgeColor}");
            Material eyeMaterial = Resources.Load<Material>(MATERIAL_PATH + type + $"/{type}_Eyes");

            if (clone.TryGetComponent(out CharacterComponentProvider provider))
            {
                MeshRenderer meshRenderer = provider.GetMeshRenderer();
                Material[] materials = { sideMaterial, frontMaterial, eyeMaterial };
                meshRenderer.materials = materials;
            }

            return clone;
        }

        public void TunningCharacter(GameObject currentPlayer, int sideColor, int edgeColor, string type = "Cubix")
        {
            Material sideMaterial = Resources.Load<Material>(MATERIAL_PATH + type + $"/{type}_Col2 {sideColor}");
            Material frontMaterial = Resources.Load<Material>(MATERIAL_PATH + type + $"/{type}_Col1 {edgeColor}");
            Material eyeMaterial = Resources.Load<Material>(MATERIAL_PATH + type + $"/{type}_Eyes");

            if (currentPlayer.TryGetComponent(out CharacterComponentProvider provider))
            {
                MeshRenderer meshRenderer = provider.GetMeshRenderer();
                Material[] materials = { sideMaterial, frontMaterial, eyeMaterial };
                meshRenderer.materials = materials;
            }
        }
    }
}
