using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Scoreboard : MonoBehaviour
    {
        public Grid grid;
        public Text daylightText;
        public Text rationsText;

        int rationsToCreate = 4;
        int rations;

        public float daylightSpeed = 0.3f;
        public float maxDaylight = 100.0f;

        float daylight;

        public float Daylight => daylight;

        private void Awake()
        {
            daylight = maxDaylight;
        }

        private void Start()
        {
            grid.CreateResources(rationsToCreate);
        }

        public void SpendDaylight()
        {
            daylight -= daylightSpeed * Time.deltaTime;
            daylightText.text = $"Daylight: {Mathf.RoundToInt(daylight)}";
        }

        public void AddRations(int rations)
        {
            this.rations += rations;
            rationsText.text = $"Rations: {this.rations}";
            if (this.rations % rationsToCreate == 0)
            {
                grid.CreateResources(rationsToCreate);
                daylight = maxDaylight;
            }
        }
    }

}