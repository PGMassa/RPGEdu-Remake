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
    [Header("Pause Menu Components")]
    [SerializeField] private Canvas PauseMenuCanvas;

    [Header("HUD Components")]
    [SerializeField] private Canvas hudCanvas;
    [SerializeField] private TMP_Text interactablePrompt; // Text used to show that an object/character can be interacted with

    [Header("Dialogue System Components")]
    [SerializeField] private Canvas dialogueCanvas;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private List<GameObject> dialogueChoiceButtons;

    private DialogueUI dialogueUI;
    private HUDUI hudUI;

    private void Awake()
    {
        dialogueUI = new DialogueUI(dialogueCanvas, dialogueBox, dialogueText, dialogueChoiceButtons);
        hudUI = new HUDUI(hudCanvas, interactablePrompt);
    }

    private void OnEnable()
    {
        Cursor.visible = true;
        hudUI.StartHUDUI();

        // Doing it on a coroutine to avoid "execution order" shenanigans
        StartCoroutine(SubscribeCallbacks());
    }

    private IEnumerator SubscribeCallbacks()
    {
        yield return new WaitUntil(() => EventManager.Instance != null);

        // Subscribing to dialogue events
        EventManager.Instance.dialogueEvents.OnDialogueStarted += StartDialogueUI;
        EventManager.Instance.dialogueEvents.OnDialogueEnded += CloseDialogueUI;
        EventManager.Instance.dialogueEvents.OnNextDialogueLine += UpdateDialogueText;
        EventManager.Instance.dialogueEvents.OnDialogueChoicesEnabled += DisplayDialogueChoices;
        EventManager.Instance.dialogueEvents.OnDialogueChoicesDisabled += HideDialogueChoices;

        // Subscribing to NPC events
        EventManager.Instance.uiEvents.OnNPCInterfaceChangeRequest += UpdateDialogueBoxInterface;

        // Subscribing to InteractionPrompt events
        EventManager.Instance.uiEvents.OnDisplayInteractionPromptRequest += (requesterID, message) => hudUI.ShowInteractionPrompt(message);
        EventManager.Instance.uiEvents.OnHideInteractionPromptRequest += (requesterID, message) => hudUI.HideInteractionPrompt(message);

        // Notify EventManager that UIManager is listening
        EventManager.Instance.internalEvents.ManagerStartedListening(gameObject.name);
    }

    // Callback methods - Using explicit methods intead of anonymous methods for better readibility
    public void StartDialogueUI()
    {
        dialogueUI.StartDialogueUI();
        hudUI.CloseHUDUI();
    }

    public void CloseDialogueUI()
    {
        dialogueUI.CloseDialogueUI();
        hudUI.StartHUDUI();
    }

    public void UpdateDialogueText(string nextLine)
    {
        dialogueUI.UpdateDialogueText(nextLine);
    }

    public void DisplayDialogueChoices(List<string> options)
    {
        dialogueUI.DisplayDialogueChoices(options);
    }

    public void HideDialogueChoices()
    {
        dialogueUI.HideDialogueChoices();
    }

    public void UpdateDialogueBoxInterface(Sprite dialogueBox)
    {
        dialogueUI.UpdateDialogueBoxInterface(dialogueBox);
    }

}
