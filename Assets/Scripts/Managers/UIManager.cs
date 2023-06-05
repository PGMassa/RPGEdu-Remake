using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * This singleton class is responsible for managing other UI related classes, as well as communicating
 * with the rest of the game.
 * This class does not control UI elements directly.
 */
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } // This class is a singleton

    [Header("Dialogue System Components")]
    [SerializeField] private TMP_Text interactablePrompt; // Text used to show that an object/character can be interacted with

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    private DialogueUI dialogueUI;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one UIManager component was found on this scene");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        Cursor.visible = false;
    }

    private void Start()
    {
        dialogueUI = new DialogueUI(interactablePrompt, dialogueBox, dialogueText);
    }


    // InteractionPrompt-related methods
    public void ShowInteractionPrompt(string interatableText)
    {
        dialogueUI.ShowInteractionPrompt(interatableText);
    }

    public void HideInterationPrompt(string interactableText)
    {
        dialogueUI.HideInteractionPrompt(interactableText);
    }


    // DialogueBox-related methods
    public void StartDialogueUI(string firstLine = "")
    {
        dialogueUI.StartDialogueUI(firstLine);
    }

    public void UpdateDialogueText(string dialogueLine)
    {
        dialogueUI.UpdateDialogueText(dialogueLine);
    }

    public void CloseDialogueUI()
    {
        dialogueUI.CloseDialogueUI();
    }

}
