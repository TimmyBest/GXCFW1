using UnityEngine;

namespace MarketplaceSDK.Example.Game.Models
{
    public class GridModel
    {
        public Vector3[,] cellPositions { get; } = new Vector3[10, 10];

        private float _startXPos = -4.5f;
        private float _startYPos = -4.5f;
        private float _startHeight = 0.5f;
        private float _cellSize = 1.0f;
        private int _gridSize = 10;

        public GridModel(float scaleY)
        {
            _startHeight = scaleY * 0.5f;

            InitCell();
        }

        public GridModel()
        {
            InitCell();
        }

        private void InitCell()
        {
            for (int i = 0; i < _gridSize; i++)
            {
                for (int j = 0; j < _gridSize; j++)
                {
                    float x = _startXPos + i * _cellSize * _cellSize;
                    float z = _startYPos + j * _cellSize * _cellSize;
                    cellPositions[i, j] = new Vector3(x, _startHeight, z);
                }
            }
        }
    }
}
