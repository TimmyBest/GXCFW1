using UnityEngine;

namespace MarketplaceSDK.Example.Interfaces
{
    public interface IPersonCreator
    {
        GameObject CreatePerson(GameObject prefab, Vector3 position, string type, int number);
    }
}