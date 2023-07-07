using System.Collections.Generic;
using UnityEngine;

namespace KeepsakeSDK.Example.Game.Models
{
    public class Cell
    {
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }

        public Cell(int x, int y)
        {
            this.RowNumber = x;
            this.ColumnNumber = y;
        }

        public Cell() { }

    }

    public class CritterCell : Cell
    {
        public CritterCell(int x, int y) : base(x, y)
        {
            this.RowNumber = x;
            this.ColumnNumber = y;

            for(int i = 0; i < 9; i++)
            {
                ListOfCritters.Add(new CellModel(i));
            }
        }

        public List<CellModel> ListOfCritters { get; set; } = new();

        public bool ExistFreeCell()
        {
            for (int i = 0; i < ListOfCritters.Count; i++)
            {
                if (!ListOfCritters[i].engaged)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class CellModel
    {
        public GameObject critter;
        public int number;
        public Vector3 pos;
        public bool engaged;

        public CellModel(GameObject critter, int number, Vector3 pos)
        {
            this.critter = critter;
            this.number = number;
            this.pos = pos;

            engaged = true;
        }

        public CellModel(int number) { this.number = number; }
    }
}
