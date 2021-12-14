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
        bool isChoosing = false;

		private void Awake()
		{
            currentNode = currentDialogue.RootNode;
		}

        public bool IsChoosing => isChoosing;

		public string GetText() {

            if (currentNode == null)
                return string.Empty;

            return currentNode.Text;
        }

        public IEnumerable<DialogueNode> GetChoices()
            => currentDialogue
                .GetChildren(currentNode)
                .Where(child => child.IsPlayerSpeaking);

        public void SelectChoice(DialogueNode chosenNode)
		{
            currentNode = chosenNode;
            isChoosing = false;
            Next();
		}

        public void Next()
		{
            var children = currentDialogue.GetChildren(currentNode);
            if (!children.Any())
                return;

            if (children.Any(child => child.IsPlayerSpeaking))
			{
                isChoosing = true;
                return;
			}

            var randomChild = Random.Range(0, children.Count());
            currentNode = children.ElementAt(randomChild);
		}

        public bool HasNext()
            => currentDialogue.GetChildren(currentNode).Any();
    }
}