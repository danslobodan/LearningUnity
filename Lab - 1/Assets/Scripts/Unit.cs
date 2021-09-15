using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Unit : MonoBehaviour
    {

        public Transform target;
        public float speed = 5;
        Vector3[] path;
        int targetIndex;

        private void Start()
        {
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        }

        private void OnPathFound(Vector3[] path, bool success)
        {
            Debug.Log($"Path Found {success}");
            if (success)
            {
                this.path = path;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        IEnumerator FollowPath()
        {
            Vector3 currentWaypoint = path[0];

            while(true)
            {
                if (transform.position == currentWaypoint) 
                { 
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;
            }
        }

        public void OnDrawGizmos()
        {
            if (path != null)
            {
                Debug.Log(path.Length);
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