using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * This class is responsible for managing other UI related classes.
 * This class does not control UI elements directly.
 */
public class UIManager : MonoBehaviour
{
    [Header("Dialogue System Components")]
    [SerializeField] private TMP_Text interactablePrompt; // Text used to show that an object/character can be interacted with

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private List<GameObject> dialogueChoiceButtons;

    private DialogueUI dialogueUI;

    private void Awake()
    {
        dialogueUI = new DialogueUI(interactablePrompt, dialogueBox, dialogueText, dialogueChoiceButtons);
    }

    private void OnEnable()
    {
        Cursor.visible = true;

        // Doing it on a coroutine to avoid "execution order" shenanigans
        StartCoroutine(SubscribeCallbacks());
    }

    private IEnumerator SubscribeCallbacks()
    {
        yield return new WaitUntil(() => EventManager.Instance != null);

        // Subscribing to dialogue events
        EventManager.Instance.dialogueEvents.OnDialogueStarted += dialogueUI.StartDialogueUI;
        EventManager.Instance.dialogueEvents.OnDialogueEnded += dialogueUI.CloseDialogueUI;
        EventManager.Instance.dialogueEvents.OnNextDialogueLine += i => dialogueUI.UpdateDialogueText(i);
        EventManager.Instance.dialogueEvents.OnDialogueChoicesEnabled += i => dialogueUI.DisplayDialogueChoices(i);
        EventManager.Instance.dialogueEvents.OnDialogueChoicesDisabled += dialogueUI.HideDialogueChoices;

        // Subscribing to NPC events
        EventManager.Instance.uiEvents.OnNPCInterfaceChangeRequest += (dialogueBox) => dialogueUI.UpdateDialogueBoxInterface(dialogueBox);

        // Subscribing to InteractionPrompt events
        EventManager.Instance.uiEvents.OnDisplayInteractionPromptRequest += (requesterID, message) => dialogueUI.ShowInteractionPrompt(message);
        EventManager.Instance.uiEvents.OnHideInteractionPromptRequest += (requesterID, message) => dialogueUI.HideInteractionPrompt(message);

        // Notify EventManager that UIManager is listening
        EventManager.Instance.internalEvents.ManagerStartedListening(gameObject.name);
    }

    private void OnDisable()
    {
        // Unsubscribing to dialogue events
        EventManager.Instance.dialogueEvents.OnDialogueStarted -= dialogueUI.StartDialogueUI;
        EventManager.Instance.dialogueEvents.OnDialogueEnded -= dialogueUI.CloseDialogueUI;
        EventManager.Instance.dialogueEvents.OnNextDialogueLine -= i => dialogueUI.UpdateDialogueText(i);
        EventManager.Instance.dialogueEvents.OnDialogueChoicesEnabled -= i => dialogueUI.DisplayDialogueChoices(i);
        EventManager.Instance.dialogueEvents.OnDialogueChoicesDisabled -= dialogueUI.HideDialogueChoices;

        // Unsubscribing to NPC events
        EventManager.Instance.uiEvents.OnNPCInterfaceChangeRequest += (dialogueBox) => dialogueUI.UpdateDialogueBoxInterface(dialogueBox);

        // Unsubscribing to InteractionPrompt events
        EventManager.Instance.uiEvents.OnDisplayInteractionPromptRequest -= (requesterID, message) => dialogueUI.ShowInteractionPrompt(message);
        EventManager.Instance.uiEvents.OnHideInteractionPromptRequest -= (requesterID, message) => dialogueUI.HideInteractionPrompt(message);

        // Notify EventManager that InputManager is not listening anymore
        EventManager.Instance.internalEvents.ManagerStoppedListening(gameObject.name);
    }
}
