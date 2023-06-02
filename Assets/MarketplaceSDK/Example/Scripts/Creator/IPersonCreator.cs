using UnityEngine;

namespace MarketplaceSDK.Example.Interfaces
{
    public interface IPersonCreator
    {
        GameObject CreatePerson(GameObject prefab, Vector3 position, string type, int sideColor, int edgeColor);
        void TunningPerson(GameObject currentPlayer, int sideColor, int edgeColor, string type = "Cubix");
    }
}