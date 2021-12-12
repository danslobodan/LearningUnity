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
		[SerializeField] Vector2 offset = new Vector2(0, 200);

		public IEnumerable<DialogueNode> Nodes => nodes;

		Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

		private void OnValidate()
		{
			UpdateLookup();
		}

		public IEnumerable<DialogueNode> GetChildren(DialogueNode parentNode)
			=> parentNode.Children
				.Where(id => nodeLookup.ContainsKey(id))
				.Select(id => nodeLookup[id]);

#if UNITY_EDITOR

		public void CreateNode(DialogueNode parentNode)
		{
			var newNode = MakeNode(parentNode);
			parentNode.AddChild(newNode.name);
			Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node.");
			Undo.RecordObject(this, "Added Dialogue Node");
			AddNode(newNode);
		}

		private DialogueNode MakeNode(DialogueNode parentNode)
		{
			var newNode = CreateInstance<DialogueNode>();
			newNode.name = Guid.NewGuid().ToString();
			if (parentNode == null)
				return newNode;

			newNode.SetIsPlayerSpeaking(!parentNode.IsPlayerSpeaking);
			newNode.SetPosition(parentNode.Rect.position + offset);
			return newNode;
		}

		private void AddNode(DialogueNode node)
		{
			nodes.Add(node);
			UpdateLookup();
		}

		public void DeleteNode(DialogueNode node)
		{
			if (nodes.Count <= 1)
				return;

			Undo.RecordObject(this, "Removed Dialogue Node");
			RemoveFromChildren(node);
			nodes.Remove(node);
			UpdateLookup();
			Undo.DestroyObjectImmediate(node);
		}

		private void RemoveFromChildren(DialogueNode node) 
			=> nodes.ForEach(parent => parent.RemoveChild(node.name));
#endif
		private void UpdateLookup()
			=> nodeLookup = nodes
				.GroupBy(node => node.name)
				.ToDictionary(node => node.First().name, node => node.First());

		public void OnBeforeSerialize()
		{
#if UNITY_EDITOR
			if (!nodes.Any())
			{
				var rootNode = MakeNode(null);
				AddNode(rootNode);
			}

			if (AssetDatabase.GetAssetPath(this) == string.Empty)
				return;

			nodes.ForEach(node =>
			{
				if (AssetDatabase.GetAssetPath(node) == string.Empty)
					AssetDatabase.AddObjectToAsset(node, this);
			});
#endif
		}

		public void OnAfterDeserialize()
		{
		}
	}
}