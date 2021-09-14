using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Grid
    {
        private readonly int gridSize;
        private readonly Tile[,] tiles;
        public int GridSize { get { return gridSize; } }
        public Tile[,] Tiles { get { return tiles; } }

        public Grid(int gridSize)
        {
            this.gridSize = gridSize;
            this.tiles = GenerateGrid(gridSize);
        }

        Tile[,] GenerateGrid(int gridSize)
        {
            Tile[,] grid = new Tile[gridSize, gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    var newTile = new Tile
                    {
                        PosX = i,
                        PosY = j,
                        Blocked = Random.Range(0, 10) > 3
                    };
                    grid[i, j] = newTile;
                }
            }
            return grid;
        }
    }
}