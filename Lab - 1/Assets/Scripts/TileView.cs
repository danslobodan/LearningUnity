using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class TileView : MonoBehaviour
    {
        public Tile tile;
        public GridManager gridManager;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseUp()
        {
            this.gridManager.TilePressed(tile.PosX, tile.PosY);
        }
    }
}