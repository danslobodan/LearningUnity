using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PathRequestManager : MonoBehaviour
    {
        Queue<PathRequest> pathRequests = new Queue<PathRequest>();
        PathRequest currentPathRequest;

        static PathRequestManager instance;
        Pathfinding pathfinding;

        bool isProcessingPath;

        private void Awake()
        {
            instance = this;
            pathfinding = GetComponent<Pathfinding>();
        }

        public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
            instance.pathRequests.Enqueue(newRequest);
            instance.TryProcessNext();
        }

        private void TryProcessNext()
        {
            if (!isProcessingPath && pathRequests.Count > 0)
            {
                currentPathRequest = pathRequests.Dequeue();
                isProcessingPath = true;
                pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
            }
        }

        public void FinishedProcessingPath(Vector3[] path, bool success)
        {
            currentPathRequest.callback(path, success);
            isProcessingPath = false;
            TryProcessNext();
        }

        struct PathRequest
        {
            public Vector3 pathStart;
            public Vector3 pathEnd;
            public Action<Vector3[], bool> callback;

            public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
            {
                this.pathStart = pathStart;
                this.pathEnd = pathEnd;
                this.callback = callback;
            }
        }
    }
}