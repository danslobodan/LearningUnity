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
		[SerializeField] GameObject AIResponse;
		[SerializeField] Transform choiceRoot;
		[SerializeField] GameObject choicePrefab;
		[SerializeField] Button quitButton;

		private void Start()
		{
			playerConversant = GameObject
				.FindGameObjectWithTag("Player")
				.GetComponent<PlayerConversant>();

			playerConversant.onConversationUpdated += UpdateUI;

			UpdateUI();
			nextButton.onClick.AddListener(() => playerConversant.Next());
			quitButton.onClick.AddListener(() => playerConversant.Quit());
		}

		private void UpdateUI()
		{
			gameObject.SetActive(playerConversant.IsActive);

			if (!playerConversant.IsActive)
				return;

			AIResponse.SetActive(!playerConversant.IsChoosing);
			choiceRoot.gameObject.SetActive(playerConversant.IsChoosing);

			if (playerConversant.IsChoosing)
				UpdateChoices();
			else
				UpdateAIText();
		}

		private void UpdateAIText()
		{
			AIText.text = playerConversant.GetText();
			nextButton.gameObject.SetActive(playerConversant.HasNext());
		}

		private void UpdateChoices()
		{
			foreach (Transform item in choiceRoot)
			{
				Destroy(item.gameObject);
			}

			playerConversant.GetChoices()
				.ToList()
				.ForEach(choice =>
				{
					var choiceInstance = Instantiate(choicePrefab, choiceRoot);
					choiceInstance.GetComponentInChildren<TextMeshProUGUI>().text = choice.Text;
					var button = choiceInstance.GetComponentInChildren<Button>();
					button.onClick.AddListener(() => playerConversant.SelectChoice(choice));
				});
		}
	}
}
