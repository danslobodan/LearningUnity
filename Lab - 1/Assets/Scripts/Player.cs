using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        public float speed = 5;
        Vector3[] path;
        int targetIndex;

        public void OnPathFound(Vector3[] path, bool success)
        {
            if (success)
            {
                this.path = path;
                targetIndex = 0;
            }
        }

        void Update()
        {
            if (path is null || path.Length == 0 || targetIndex >= path.Length)
                return;

            Vector3 currentWaypoint = path[targetIndex];
            if (transform.position != currentWaypoint)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, currentWaypoint, speed * Time.deltaTime);
            }

            if(transform.position == currentWaypoint && targetIndex < path.Length)
                targetIndex++;
        }

        public void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(path[i], new Vector3(1, 0.2f, 1));

                    if (i == targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }
    }
}