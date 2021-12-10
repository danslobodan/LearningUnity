using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] DialogueNode[] nodes;
    }
}