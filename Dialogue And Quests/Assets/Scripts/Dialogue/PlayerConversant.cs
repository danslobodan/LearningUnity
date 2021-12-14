using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        Dialogue currentDialogue;
        DialogueNode currentNode;
        bool isChoosing = false;

        public event Action onConversationUpdated;

        public bool IsActive => currentDialogue != null;

        public bool IsChoosing => isChoosing;

		public string GetText() {

            if (currentNode == null)
                return string.Empty;

            return currentNode.Text;
        }

        public void StartDialogue(Dialogue dialogue)
		{
            currentDialogue = dialogue;
            currentNode = dialogue.RootNode;
            onConversationUpdated();
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
                onConversationUpdated();
                return;
			}

            var randomChild = UnityEngine.Random.Range(0, children.Count());
            currentNode = children.ElementAt(randomChild);
            onConversationUpdated();
        }

        public bool HasNext()
            => currentDialogue.GetChildren(currentNode).Any();

        public void Quit()
		{
            currentDialogue = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
		}
    }
}