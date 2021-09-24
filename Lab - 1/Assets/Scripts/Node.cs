using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Node : IComparable<Node>
    {
        public bool walkable;
        public Vector3 worldPosition;

        public int gridX;
        public int gridY;

        public int gCost;
        public int hCost;

        public Node parent;

        public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
        {
            this.walkable = walkable;
            this.worldPosition = worldPosition;
            this.gridX = gridX;
            this.gridY = gridY;
        }

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public int CompareTo(Node other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(other.hCost);
            }

            return -compare;
        }

        public override string ToString()
        {
            return $"{gridX} {gridY}"; // $"{fCost}"; 
        }
    }

}