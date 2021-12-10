using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [Serializable]
    public class DialogueNode
    {
        public string uniqueID;
        public string text;
        public string[] children;
    }
}