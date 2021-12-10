using System;
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
			{
				var root = new DialogueNode();
				Add(root);
			}
		}
#endif

		private void OnValidate()
		{
			UpdateLookup();
		}

		public IEnumerable<DialogueNode> GetChildren(DialogueNode parentNode)
		{
			return parentNode.Children
				.Where(id => nodeLookup.ContainsKey(id))
				.Select(id => nodeLookup[id]);
		}

		public void CreateNode(DialogueNode parentNode)
		{
			var newNode = new DialogueNode();
			parentNode.Children.Add(newNode.uniqueID);
			Add(newNode);
		}

		public void DeleteNode(DialogueNode node)
		{
			RemoveFromChildren(node);
			nodes.Remove(node);
			UpdateLookup();
		}

		private void RemoveFromChildren(DialogueNode node) 
		{
			nodes.ForEach(parent =>
			{
				parent.Children.Remove(node.uniqueID);
			});
		}

		private void Add(DialogueNode node)
		{
			nodes.Add(node);
			UpdateLookup();
		}

		private void UpdateLookup()
		{
			nodeLookup = nodes
				.GroupBy(node => node.uniqueID)
				.ToDictionary(node => node.First().uniqueID, node => node.First());
		}
	}
}