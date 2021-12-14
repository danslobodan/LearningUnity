using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;
using System.Linq;

namespace RPG.UI
{
	public class DialogueUI : MonoBehaviour
	{
		PlayerConversant playerConversant;
		[SerializeField] TextMeshProUGUI AIText;
		[SerializeField] Button nextButton;
		[SerializeField] Transform choiceRoot;
		[SerializeField] GameObject choicePrefab;

		private void Start()
		{
			playerConversant = GameObject
				.FindGameObjectWithTag("Player")
				.GetComponent<PlayerConversant>();

			UpdateUI();
			nextButton.onClick.AddListener(Next);
		}

		private void Next()
		{
			playerConversant.Next();
			UpdateUI();
		}

		private void UpdateUI()
		{
			AIText.text = playerConversant.GetText();
			nextButton.gameObject.SetActive(playerConversant.HasNext());
			foreach (Transform item in choiceRoot)
			{
				Destroy(item.gameObject);
			}

			playerConversant.GetChoices()
				.ToList()
				.ForEach(choice =>
				{
					var choiceUI = Instantiate(choicePrefab, choiceRoot);
					choiceUI.GetComponentInChildren<TextMeshProUGUI>().text = choice;
				});
		}
	}
}
