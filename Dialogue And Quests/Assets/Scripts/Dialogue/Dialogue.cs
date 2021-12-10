using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Dialogue")]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();

		public IEnumerable<DialogueNode> Nodes => nodes;

		Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
		private void Awake()
		{
			if (nodes.Count == 0)
				CreateNode();
		}
#endif

		private void OnValidate()
		{
			UpdateLookup();
		}

		public IEnumerable<DialogueNode> GetChildren(DialogueNode parentNode)
			=> parentNode.Children
				.Where(id => nodeLookup.ContainsKey(id))
				.Select(id => nodeLookup[id]);

		public DialogueNode CreateNode()
		{
			var newNode = CreateInstance<DialogueNode>();
			newNode.name = Guid.NewGuid().ToString();
			Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node.");
			nodes.Add(newNode);
			UpdateLookup();
			return newNode;
		}

		public void CreateNode(DialogueNode parentNode)
		{
			var newNode = CreateNode();
			parentNode.Children.Add(newNode.name);
		}

		public void DeleteNode(DialogueNode node)
		{
			if (nodes.Count <= 1)
				return;

			RemoveFromChildren(node);
			nodes.Remove(node);
			UpdateLookup();
			Undo.DestroyObjectImmediate(node);
		}

		private void RemoveFromChildren(DialogueNode node) 
			=> nodes.ForEach(parent => parent.Children.Remove(node.name));

		private void UpdateLookup()
			=> nodeLookup = nodes
				.GroupBy(node => node.name)
				.ToDictionary(node => node.First().name, node => node.First());

		public void OnBeforeSerialize()
		{
			if (AssetDatabase.GetAssetPath(this) == string.Empty)
				return;

			nodes.ForEach(node =>
			{
				if (AssetDatabase.GetAssetPath(node) == string.Empty)
					AssetDatabase.AddObjectToAsset(node, this);
			});
		}

		public void OnAfterDeserialize()
		{
		}
	}
}