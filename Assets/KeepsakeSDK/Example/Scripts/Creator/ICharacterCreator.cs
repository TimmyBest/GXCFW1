using UnityEngine;

namespace KeepsakeSDK.Example.Interfaces
{
    public interface ICharacterCreator
    {
        GameObject CreateCharacter(GameObject prefab, Vector3 position, string type, int sideColor, int edgeColor);
        void TunningCharacter(GameObject currentPlayer, int sideColor, int edgeColor, string type = "Cubix");
    }
}