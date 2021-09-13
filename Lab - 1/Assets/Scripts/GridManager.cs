using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Assets.Scripts
{
    public class GridManager : MonoBehaviour
    {
        public TileView tileView;
        public Ball ball;
        private Grid grid;
        private TileView[,] gridView;

        const int GRID_SIZE = 4;

        private Vector3 offset = new Vector3(10.3f, 0, 10.3f);

        // Start is called before the first frame update
        void Start()
        {
            grid = new Grid(GRID_SIZE);
            gridView = GenerateGridView(grid);
            ball.transform.position = new Vector3(0, 2.5f, 0);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TilePressed(int i, int j)
        {
            var tilePosition = gridView[i, j].transform.position;
            ball.MoveBall(tilePosition);
        }

        public TileView[,] GenerateGridView(Grid grid)
        {
            var gridView = new TileView[grid.GridSize, grid.GridSize];
            for (int i = 0; i < grid.GridSize; i++)
            {
                for (int j = 0; j < grid.GridSize; j++)
                {
                    var tileView = Instantiate(this.tileView);
                    tileView.transform.position = new Vector3(
                        offset.x * i, 0, offset.z * j);
                    tileView.tile = grid.Tiles[i, j];
                    tileView.transform.SetParent(this.transform);
                    tileView.gameObject.name = $"Tile {i+1} {j+1}";
                    gridView[i, j] = tileView;
                }
            }

            return gridView;
        }
    }
}
