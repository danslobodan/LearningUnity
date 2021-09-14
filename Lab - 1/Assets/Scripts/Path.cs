using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Path
    {
        private Tile[,] open;
        private Tile[,] closed;

        public Path(Grid grid, Tile fromTile, Tile toTile)
        {
            open = new Tile[grid.GridSize, grid.GridSize];
            grid.Tiles.CopyTo(open, 0);

            closed = new Tile[grid.GridSize, grid.GridSize];

            
        }

        private class Node
        {
            public int F { get; set; }
            public int G { get; set; }
            public int MyProperty { get; set; }
        }
    }
}