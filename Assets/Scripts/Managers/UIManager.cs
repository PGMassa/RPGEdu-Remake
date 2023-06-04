using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } // This class is a singleton

    [SerializeField] private TMP_Text interactablePrompt; // Text used to show that an object/character can be interacted with

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    // This bool is true when there is an object nearby asking to show the interactable prompt.
    // It keeps being true even when the dialogueBox disables the interactable prompt gameObject
    private bool isInteractablePromptNeeded;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Default values for the interface
        Cursor.visible = false;
        interactablePrompt.text = "";
        interactablePrompt.gameObject.SetActive(false);
        isInteractablePromptNeeded = false;
        dialogueText.text = "";
        dialogueBox.SetActive(false);
    }

    // Show interactable prompt UI with a new text
    public void ShowInteractablePromptText(string interatableText)
    {
        interactablePrompt.text = interatableText;
        isInteractablePromptNeeded = true;

        if(!dialogueBox.activeInHierarchy) interactablePrompt.gameObject.SetActive(true); //dialogue box overrides the prompt
    }

    // Hide interactable promtp UI
    public void HideInteratablePromptText()
    {
        interactablePrompt.text = "";
        isInteractablePromptNeeded = false;

        interactablePrompt.gameObject.SetActive(false);
    }

    public void ShowDialogueBox(string firstLine = "")
    {
        dialogueText.text = firstLine;
        dialogueBox.SetActive(true);
        interactablePrompt.gameObject.SetActive(false);
    }

    public void HideDialogueBox()
    {
        dialogueText.text = "";
        dialogueBox.SetActive(false);
        if (isInteractablePromptNeeded) interactablePrompt.gameObject.SetActive(true);
    }


}
