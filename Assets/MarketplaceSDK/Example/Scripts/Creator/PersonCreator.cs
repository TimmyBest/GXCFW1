using MarketplaceSDK.Example.Game.Provider;
using MarketplaceSDK.Example.Interfaces;
using System;
using UnityEditor;
using UnityEngine;

namespace MarketplaceSDK.Example.Game.Creator
{
    public class PersonCreator : IPersonCreator
    {
        private const string MATERIAL_PATH = "Assets/MarketplaceSDK/Example/Materials/";

        public GameObject CreatePerson(GameObject prefab, Vector3 position, string type, int sideColor, int edgeColor)
        {
            GameObject clone = GameObject.Instantiate(prefab, position, prefab.transform.rotation);

            Material sideMaterial = AssetDatabase.LoadAssetAtPath<Material>(MATERIAL_PATH + type + $"/{type}_Col2 {sideColor}.mat");
            Material frontMaterial = AssetDatabase.LoadAssetAtPath<Material>(MATERIAL_PATH + type + $"/{type}_Col1 {edgeColor}.mat");
            Material eyeMaterial = AssetDatabase.LoadAssetAtPath<Material>(MATERIAL_PATH + type + $"/{type}_Eyes.mat");

            if (clone.TryGetComponent(out PersonComponentProvider provider))
            {
                MeshRenderer meshRenderer = provider.GetMeshRenderer();
                Material[] materials = { sideMaterial, frontMaterial, eyeMaterial };
                meshRenderer.materials = materials;
            }

            return clone;
        }
    }
}
