using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
	public class AIConversant : MonoBehaviour, IRaycastable
	{
		[SerializeField] Dialogue dialogue;

		private void Start()
		{	
		}

		public CursorType GetCursorType()
		{
			return CursorType.Dialogue;
		}

		public bool HandleRaycast(PlayerController callingController)
		{
			if (dialogue == null)
				return false;

			if (Input.GetMouseButtonDown(0))
			{
				var playerConversant = callingController.gameObject.GetComponent<PlayerConversant>();
				playerConversant.StartDialogue(dialogue);
			}

			return true;
		}
	}

}