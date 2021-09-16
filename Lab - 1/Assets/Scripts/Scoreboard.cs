using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Scoreboard : MonoBehaviour
    {
        public Text daylightText;
        public Text rationsText;

        float rations;

        public void UpdateDaylight(float daylight)
        {
            this.daylightText.text = $"Daylight: {daylight}";
        }

        public void AddRations(float rations)
        {
            this.rations += rations;
            rationsText.text = $"Rations: {this.rations}";
        }
    }

}