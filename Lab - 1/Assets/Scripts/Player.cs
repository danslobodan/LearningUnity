﻿using System;
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
        public float maxDaylight = 100f;
        public float daylightSpeed = 1f;

        public Scoreboard scoreboard;

        Vector3[] path;
        int targetIndex;

        void Update()
        {
            if (path == null || path.Length == 0 || targetIndex >= path.Length
                || scoreboard.Daylight <= 0)
                return;

            Vector3 currentWaypoint = new Vector3(path[targetIndex].x, transform.position.y, path[targetIndex].z); ;
            if (transform.position != currentWaypoint)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    currentWaypoint,
                    speed * Time.deltaTime);
            }

            if (transform.position == currentWaypoint && targetIndex < path.Length)
                targetIndex++;

            scoreboard.SpendDaylight();
        }

        public void OnPathFound(Vector3[] path, bool success)
        {
            if (success)
            {
                this.path = path;
                targetIndex = 0;
            }
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