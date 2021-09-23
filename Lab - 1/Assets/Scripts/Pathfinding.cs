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

        // Heap<Node> oSet;
        Heap<Node> oSet;
        
        ICollection<Node> cSet;
        Node tNode;

        private void Awake()
        {
            requestManager = GetComponent<PathRequestManager>();
            grid = GetComponent<Grid>();
        }

        IEnumerator FindPath(Node startNode, Node targetNode)
        {
            bool pathSuccess = false;

            if (startNode.walkable && targetNode.walkable)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                Heap<Node> openSet = new Heap<Node>(grid.Size);
                // MyHeap<Node> openSet = new MyHeap<Node>();

                HashSet<Node> closedSet = new HashSet<Node>();

                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        sw.Stop();
                        // Debug.Log($"Path found {sw.ElapsedMilliseconds} ms");
                        pathSuccess = true;
                        break;
                    }

                    var neighbours = grid
                        .GetNeighbours(currentNode)
                        .Where(node => node.walkable && !closedSet.Contains(node));

                    foreach (Node neighbour in neighbours)
                    {
                        int newMovementCost = currentNode.gCost + HCost(currentNode, neighbour);

                        if (!openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCost;
                            neighbour.hCost = HCost(neighbour, tNode);
                            neighbour.parent = currentNode;
                            openSet.Add(neighbour);
                        }
                        else if (newMovementCost < neighbour.gCost)
                        {
                            neighbour.gCost = newMovementCost;
                            neighbour.hCost = HCost(neighbour, tNode);
                            neighbour.parent = currentNode;
                            openSet.UpdateItem(neighbour);
                        }
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

            while(currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            Vector3[] allWaypoints = path.Select(node => node.worldPosition)
                .Reverse()
                .ToArray();

            Vector3[] waypoints = SimplifyPath(path).Reverse().ToArray();

            return allWaypoints;
        }

        // TODO: Debug simplify path
        Vector3[] SimplifyPath(List<Node> path)
        {
            var waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;

            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionNew = 
                    new Vector2(
                        path[i-1].gridX - path[i].gridX, 
                        path[i-1].gridY - path[i].gridY);

                if (directionNew != directionOld)
                {
                    waypoints.Add(path[i].worldPosition);
                    directionOld = directionNew;
                }
            }
            return waypoints.ToArray();
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

        private void OnDrawGizmos()
        {
            if (oSet != null)
            {
                oSet.Items.ToList().ForEach(node =>
                {
                    if (node.Index == 0)
                    {
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawCube(Vec(node.Item.worldPosition, -0.1f), Vector3.one);
                    }
                    else
                    {
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawCube(Vec(node.Item.worldPosition, 0), Vector3.one);
                    }

                });
                cSet.ToList().ForEach(node =>
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(Vec(node.worldPosition, 0.1f), Vector3.one);
                });
            }
        }

        Vector3 Vec(Vector3 vec, float offset)
        {
            return new Vector3(vec.x + offset, 0, vec.z + offset);
        }
    }

}