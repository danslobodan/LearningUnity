using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Resource : MonoBehaviour
    {
        public Scoreboard scoreboard;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Player")
            {
                gameObject.SetActive(false);
                scoreboard.AddRations(1);
                Debug.Log($"Picked up rations {this.GetHashCode()}");
            }
        }
    }

}