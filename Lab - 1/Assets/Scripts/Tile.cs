using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Tile
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public bool Blocked { get; set; }

        // TODO: make this lazy
        private static ICollection<Tile> zeroNeighbours = new List<Tile>()
        {
            new Tile() { PosX = -1, PosY = -1 },
            new Tile() { PosX = -1, PosY = 0 },
            new Tile() { PosX = -1, PosY = 1 },
            new Tile() { PosX = 0, PosY = 1 },
            new Tile() { PosX = 1, PosY = 1 },
            new Tile() { PosX = 1, PosY = 0 },
            new Tile() { PosX = 1, PosY = -1 },
            new Tile() { PosX = 0, PosY = -1 },
            new Tile() { PosX = -1, PosY = -1 },
            new Tile() { PosX = -1, PosY = 0 },
        };

        // TODO: make this lazy
        public ICollection<Tile> Neighbours
        {
            get
            {
                var tiles = zeroNeighbours.Select(tile =>
                {
                    return new Tile() { PosX = tile.PosX + this.PosX, PosY = tile.PosY + this.PosY };
                }).ToList();
                return tiles;
            }
        }
    }
}
