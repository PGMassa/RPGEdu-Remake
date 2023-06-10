using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * This singleton class is responsible for managing other UI related classes.
 * This class does not control UI elements directly.
 */
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } // This class is a singleton

    [Header("Dialogue System Components")]
    [SerializeField] private TMP_Text interactablePrompt; // Text used to show that an object/character can be interacted with

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private List<GameObject> dialogueChoiceButtons;

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
            dialogueUI = new DialogueUI(interactablePrompt, dialogueBox, dialogueText, dialogueChoiceButtons);
        }

        Cursor.visible = true;
    }

    private void OnEnable()
    {
        // Doing it on a coroutine to avoid "execution order" shenanigans
        StartCoroutine(SubscribeCallbacks());
    }

    private IEnumerator SubscribeCallbacks()
    {
        // Subscribing Dialogue-related callbacks
        yield return new WaitUntil(() => DialogueManager.Instance != null);

        DialogueManager.Instance.OnDialogueStarted += dialogueUI.StartDialogueUI;
        DialogueManager.Instance.OnDialogueEnded += dialogueUI.CloseDialogueUI;
        DialogueManager.Instance.OnNextDialogueLine += i => dialogueUI.UpdateDialogueText(i);
        DialogueManager.Instance.OnDialogueChoicesEnabled += i => dialogueUI.DisplayDialogueChoices(i);
        DialogueManager.Instance.OnDialogueChoicesDisabled += dialogueUI.HideDialogueChoices;

        // Subscribing Interactable-related callbakcs
        yield return new WaitUntil(() => InteractablesManager.Instance != null);

        InteractablesManager.Instance.OnDisplayInteractionPromptRequested += (applicant, message) => dialogueUI.ShowInteractionPrompt(message);
        InteractablesManager.Instance.OnHideInteractionPromptRequested += (applicant, message) => dialogueUI.HideInteractionPrompt(message);
    }

    private void OnDisable()
    {
        // Unsubscribing Dialogue-related callbacks
        DialogueManager.Instance.OnDialogueStarted -= dialogueUI.StartDialogueUI;
        DialogueManager.Instance.OnDialogueEnded -= dialogueUI.CloseDialogueUI;
        DialogueManager.Instance.OnNextDialogueLine -= i => dialogueUI.UpdateDialogueText(i);
        DialogueManager.Instance.OnDialogueChoicesEnabled -= i => dialogueUI.DisplayDialogueChoices(i);
        DialogueManager.Instance.OnDialogueChoicesDisabled -= dialogueUI.HideDialogueChoices;

        // Unsubscribing Interactable-related callbakcs
        InteractablesManager.Instance.OnDisplayInteractionPromptRequested -= (applicant, message) => dialogueUI.ShowInteractionPrompt(message);
        InteractablesManager.Instance.OnHideInteractionPromptRequested -= (applicant, message) => dialogueUI.HideInteractionPrompt(message);
    }
}
