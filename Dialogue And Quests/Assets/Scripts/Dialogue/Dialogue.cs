using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();

		public IEnumerable<DialogueNode> Nodes => nodes;

		public DialogueNode RootNode => nodes.First();

#if UNITY_EDITOR
		private void Awake()
		{
			if (nodes.Count == 0)
			{
				nodes.Add(new DialogueNode());
			}
		}
#endif
	}
}