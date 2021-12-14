using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
	PlayerConversant playerConversant;
	[SerializeField] TextMeshProUGUI AIText;
	[SerializeField] Button nextButton;

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
	}
}
