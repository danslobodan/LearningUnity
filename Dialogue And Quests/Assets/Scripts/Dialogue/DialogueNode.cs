using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [Serializable]
    public class DialogueNode
    {
        public string uniqueID = Guid.NewGuid().ToString();
        public string text;
        [SerializeField] List<string> children = new List<string>();


        public ICollection<string> Children => children;

        public Rect rect = new Rect(0, 0, 200, 100);
    }
}