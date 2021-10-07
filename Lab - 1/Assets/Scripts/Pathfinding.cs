using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts
{
    public class Pathfinding : MonoBehaviour
    {
        PathRequestManager requestManager;
        Grid grid;

        private void Awake()
        {
            requestManager = GetComponent<PathRequestManager>();
            grid = GetComponent<Grid>();
        }

        IEnumerator FindPath(Node startNode, Node targetNode)
        {
            bool pathSuccess = false;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (startNode.walkable && targetNode.walkable)
            {
                var openSet = new Heap<Node>(); 
                var closedSet = new HashSet<Node>();

                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        sw.Stop();
                        Debug.Log($"Path found in {sw.ElapsedMilliseconds} miliseconds");
                        pathSuccess = true;
                        break;
                    }

                    var neighbours = grid
                        .GetNeighbours(currentNode)
                        .Where(node => node.walkable && !closedSet.Contains(node));

                    foreach (Node neighbour in neighbours)
                    {
                        int newMovementCost = currentNode.gCost + HCost(currentNode, neighbour);
                        bool inOpenSet = openSet.Contains(neighbour);

                        if (!inOpenSet || newMovementCost < neighbour.gCost)
                        {
                            neighbour.gCost = newMovementCost;
                            neighbour.hCost = HCost(neighbour, targetNode);
                            neighbour.parent = currentNode;
                        }

                        if (!inOpenSet)
                            openSet.Add(neighbour);
                        else if (newMovementCost < neighbour.gCost)
                            openSet.Update(neighbour);
                    }
                }
            }

            yield return null;

            Vector3[] waypoints = new Vector3[0];
            if (pathSuccess)
            {
                waypoints = RetracePath(startNode, targetNode);
            }

            requestManager.FinishedProcessingPath(waypoints, pathSuccess);
        }

        public void StartFindPath(Vector3 pathStart, Vector3 pathEnd)
        {
            Node startNode = grid.NodeFromWorldPoint(pathStart);
            Node targetNode = grid.NodeFromWorldPoint(pathEnd);

            StartCoroutine(FindPath(startNode, targetNode));
        }

        Vector3[] RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            IEnumerable<Node> simplePath = SimplifyPath(path);
            Debug.Log($"Path {string.Join(" , ", path)}");
            Debug.Log($"Simple {string.Join(" , ", simplePath)}");

            Vector3[] waypoints = simplePath
                .Select(node => node.worldPosition)
                .Reverse()
                .ToArray();

            return waypoints;
        }

        IEnumerable<Node> SimplifyPath(List<Node> path)
        {
            var baseWaypoints = new List<Node>();
            Vector2 directionOld = Vector2.zero;

            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector2 directionNew = 
                    new Vector2(
                        path[i + 1].gridX - path[i].gridX,
                        path[i + 1].gridY - path[i].gridY);

                if (directionNew != directionOld)
                {
                    baseWaypoints.Add(path[i]);
                    directionOld = directionNew;
                }
            }
            return baseWaypoints;
        }

        int HCost(Node nodeA, Node nodeB)
        {
            int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if (distanceX > distanceY)
                return HCost(distanceX, distanceY);

            return HCost(distanceY, distanceX);
        }

        int HCost(int longer, int shorter)
        {
            return 14 * shorter + 10 * (longer - shorter);
        }
    }

}