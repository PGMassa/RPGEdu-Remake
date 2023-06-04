using UnityEngine;
using TMPro;

/*
 * This singleton class is responsible for managing other UI related classes, as well as communicating
 * with the rest of the game.
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

        dialogueUI = new DialogueUI(interactablePrompt, dialogueBox, dialogueText);
    }

    public void ShowInteractionPrompt(string interatableText)
    {
        dialogueUI.ShowInteractionPrompt(interatableText);
    }

    public void HideInterationPrompt(string interactableText)
    {
        dialogueUI.HideInteractionPrompt(interactableText);
    }

    public void StartDialogue(string firstLine = "")
    {
        dialogueUI.StartDialogue(firstLine);
    }

    public void EndDialogue()
    {
        dialogueUI.EndDialogue();
    }

}
