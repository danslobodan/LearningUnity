using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Grid : MonoBehaviour
    {
        public bool displayGizmos;

        public Player player;
        public GameObject rations;

        public Transform resources;
        public List<GameObject> resourceList = new List<GameObject>();

        public Scoreboard scoreboard;

        public LayerMask walkable;
        public LayerMask unwalkableMask;

        public Vector2 gridWorldSize;
        public float nodeRadius;

        Node[,] grid;

        float nodeDiameter;
        int gridSizeX, gridSizeY;

        public int Size
        {
            get
            {
                return (int)(gridWorldSize.x * gridWorldSize.y);
            }
        }

        private void Awake()
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
        }

        private void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft =
                transform.position
                - Vector3.right * gridWorldSize.x / 2
                - Vector3.forward * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint =
                        worldBottomLeft
                        + Vector3.right * (x * nodeDiameter + nodeRadius)
                        + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    
                    bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }

        public void CreateResources(int count)
        {
            resourceList.ForEach(resource =>
            {
                Destroy(resource);
            });

            if (grid != null)
            {
                var walkable = from Node node in grid
                               where node.walkable
                               select node;

                var walkableCount = walkable.Count();

                var nodeIndices = new List<int>();
                var rand = new Random();
                while (nodeIndices.Count < count)
                {
                    var tile = Random.Range(0, 1000) % walkableCount;
                    if (!nodeIndices.Contains(tile))
                        nodeIndices.Add(tile);
                }

                nodeIndices.Select(i => walkable.ElementAt(i).worldPosition)
                    .ToList()
                    .ForEach(vec => {
                        var resource = Instantiate(rations, vec, Quaternion.identity, resources);
                        resource.GetComponent<Resource>().scoreboard = scoreboard;
                        resourceList.Add(resource);
                    });
            }
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for(int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX
                        && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            int x = GridPositionFromCoordinate(worldPosition.x, gridWorldSize.x, gridSizeX);
            int y = GridPositionFromCoordinate(worldPosition.z, gridWorldSize.y, gridSizeY);

            return grid[x, y];
        }

        private int GridPositionFromCoordinate(float position, float worldSize, int gridSize)
        {
            float percent = (position + worldSize / 2) / worldSize;
            float clamped = Mathf.Clamp01(percent);
            int gridPosition = Mathf.FloorToInt(gridSize * clamped);

            if (gridPosition > gridSize - 1)
                return gridSize - 1;

            return gridPosition; 
        }


        private Node raycastNode;

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 3000f, walkable))
                {
                    raycastNode = NodeFromWorldPoint(hit.point);
                    // Debug.Log($"Hit {hit.point}");
                    // Debug.Log($"Node {raycastNode}");
                    PathRequestManager.RequestPath(player.transform.position,
                        hit.point,
                        player.OnPathFound);
                }
                //else
                //    Debug.Log("No nit");
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (grid != null && displayGizmos)
            {
                Node playerNode = NodeFromWorldPoint(player.transform.position);

                foreach (Node node in grid)
                {
                    Gizmos.color = node.walkable ? Color.white : Color.red;
                    if (node == playerNode)
                    {
                        Gizmos.color = Color.blue;
                    }

                    if (this.raycastNode != null && this.raycastNode == node)
                    {
                        Gizmos.color = Color.cyan;
                    }

                    Gizmos.DrawCube(node.worldPosition, new Vector3(1, 0.1f, 1) * (nodeDiameter - 0.1f));
                }
            }
        }
    }

}