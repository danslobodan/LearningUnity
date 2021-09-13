using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 destination;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != destination)
        {
            transform.position = new Vector3(destination.x, transform.position.y, destination.z);
        }
    }

    public void MoveBall(Vector3 destination)
    {
        this.destination = destination;
    }
}
