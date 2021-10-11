using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        public float speed = 5f;
        private float tolerance = 0.1f;

        public Scoreboard scoreboard;

        private Vector3[] path;
        private Vector3 currentWaypoint;
        private int currentWaypointIndex;

        public void OnPathFound(Vector3[] path, bool success)
        {
            if (success && path.Length > 0)
            {
                this.path = path;
                currentWaypointIndex = 0;
                UpdateCurrentWaypoint();
            }
        }

        private void Update()
        {
            if (!HasPath() || TargetReached() || !HasDaylight())
                return;

            Move();
            scoreboard.SpendDaylight();

            if (WaypointReached(currentWaypoint))
                NextWaypoint();
        }

        private bool HasPath()
        {
            // Debug.Log($"Has path: {path != null && path.Length > 0}");
            return path != null && path.Length > 0;
        }

        private bool TargetReached()
        {
            // Debug.Log($"Target reached: {currentWaypointIndex >= path.Length}");
            return currentWaypointIndex >= path.Length;
        }

        private bool HasDaylight()
        {
            // Debug.Log($"Has daylight: {scoreboard.Daylight > 0}");
            return scoreboard.Daylight > 0;
        }

        private bool WaypointReached(Vector3 currentWaypoint)
        {
            return Math.Abs(transform.position.x - currentWaypoint.x) < tolerance
                && Math.Abs(transform.position.z - currentWaypoint.z) < tolerance;
        }

        private void NextWaypoint()
        {
            currentWaypointIndex++;
            if (TargetReached())
                return;

            UpdateCurrentWaypoint();
        }

        private void UpdateCurrentWaypoint()
        {
            currentWaypoint = new Vector3(path[currentWaypointIndex].x, transform.position.y, path[currentWaypointIndex].z);
        }

        private void Move()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                currentWaypoint,
                speed * Time.deltaTime);
        }

        public void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = currentWaypointIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(path[i], new Vector3(1, 0.2f, 1));

                    if (i == currentWaypointIndex)
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