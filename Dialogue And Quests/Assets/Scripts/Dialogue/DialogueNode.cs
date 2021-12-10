using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        public string text;
        [SerializeField] List<string> children = new List<string>();

        public ICollection<string> Children => children;

        public Rect rect = new Rect(0, 0, 200, 100);
    }
}