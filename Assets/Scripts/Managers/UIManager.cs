using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/*
 * This class is responsible for managing other UI related classes.
 * This class does not control UI elements directly.
 */
public class UIManager : MonoBehaviour
{
    [Header("Pause Menu Components")]
    [SerializeField] private Canvas pauseMenuCanvas;

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
    private PauseUI pauseUI;

    private void OnEnable()
    {
        Cursor.visible = true;

        // Doing it on a coroutine to avoid "execution order" shenanigans
        StartCoroutine(SubscribeCallbacks());
    }

    private IEnumerator SubscribeCallbacks()
    {
        yield return new WaitUntil(() => EventManager.Instance != null);

        EventManager.Instance.inputEvents.OnPausePerformed += HandlePause;

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

        // Initialize canvas controllers
        dialogueUI = new DialogueUI(dialogueCanvas, dialogueBox, dialogueText, dialogueChoiceButtons);
        hudUI = new HUDUI(hudCanvas, interactablePrompt);
        pauseUI = new PauseUI(pauseMenuCanvas);

        hudUI.StartHUDUI();
    }

    // Callback methods from the eventManager
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

    public void UpdateDialogueText(string nextLine) => dialogueUI.UpdateDialogueText(nextLine);
    public void DisplayDialogueChoices(List<string> options) => dialogueUI.DisplayDialogueChoices(options);
    public void HideDialogueChoices() => dialogueUI.HideDialogueChoices();
    public void UpdateDialogueBoxInterface(Sprite dialogueBox) => dialogueUI.UpdateDialogueBoxInterface(dialogueBox);
    public void HandlePause() => pauseUI.TogglePauseCanvas(); // Later:  Verify wich canvas in currently enabled and set pause menu based on that

    // Callback methods from buttons
    public void OnReturnToGame() => pauseUI.OnReturnToGame();
    public void OnSave() => pauseUI.OnSave();
    public void OnOpenInventory() => pauseUI.OnOpenInventory();
    public void OnOpenQuests() => pauseUI.OnOpenQuests();
    public void OnOpenStates() => pauseUI.OnOpenStates();
    public void OnOpenMap() => pauseUI.OnOpenMap();
    public void OnOpenOptions() => pauseUI.OnOpenOptions();
    public void OnExit() => pauseUI.OnExit();

}
