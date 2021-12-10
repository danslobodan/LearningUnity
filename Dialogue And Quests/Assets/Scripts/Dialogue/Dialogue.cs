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

		Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
		private void Awake()
		{
			if (nodes.Count == 0)
				nodes.Add(new DialogueNode());
		}
#endif

		private void OnValidate()
		{
			nodeLookup = nodes
				.GroupBy(node => node.uniqueID)
				.ToDictionary(node => node.First().uniqueID, node => node.First());
		}

		public IEnumerable<DialogueNode> GetChildren(DialogueNode parentNode)
		{
			return parentNode.Children
				.Where(id => nodeLookup.ContainsKey(id))
				.Select(id => nodeLookup[id]);
		}
	}
}