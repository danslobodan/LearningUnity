using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue currentDialogue;
        DialogueNode currentNode;

		private void Awake()
		{
            currentNode = currentDialogue.RootNode;
		}

		public string GetText() {

            if (currentNode == null)
                return string.Empty;

            return currentNode.Text;
        }

        public void Next()
		{
            var children = currentDialogue.GetChildren(currentNode);
            if (!children.Any())
                return;

            var randomChild = Random.Range(0, children.Count());
            currentNode = children.ElementAt(randomChild);
		}

        public bool HasNext()
		{
            return currentDialogue.GetChildren(currentNode).Any();
		}
    }
}