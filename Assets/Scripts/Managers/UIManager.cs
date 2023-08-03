using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface ICanvasController
{
    void StartCanvas();
    void CloseCanvas();

    GameObject canvasObject { get; }
}

/*
 * This class is responsible for managing other UI related classes.
 * This class does not control UI elements directly.
 */
public class UIManager : MonoBehaviour
{
    [Header("Main Menu Components")]
    [SerializeField] private Canvas mainMenuCanvas;

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
    private MainMenuUI mainMenuUI;

    private ICanvasController activeCanvas;

    private void OnEnable()
    {
        Cursor.visible = true;

        // Initializing canvas controllers
        dialogueUI = new DialogueUI(dialogueCanvas, dialogueBox, dialogueText, dialogueChoiceButtons);
        hudUI = new HUDUI(hudCanvas, interactablePrompt);
        pauseUI = new PauseUI(pauseMenuCanvas);
        mainMenuUI = new MainMenuUI(mainMenuCanvas);

        // Showing the mainMenu Canvas
        (mainMenuUI as ICanvasController).StartCanvas();
        activeCanvas = (mainMenuUI as ICanvasController);

        // Doing it on a coroutine to avoid "execution order" shenanigans
        StartCoroutine(SubscribeCallbacks());
    }

    private IEnumerator SubscribeCallbacks()
    {
        yield return new WaitUntil(() => EventManager.Instance != null);

        EventManager.Instance.inputEvents.OnPausePerformed += HandlePause;

        // Subscribing to dialogue events
        EventManager.Instance.dialogueEvents.OnDialogueStarted += () => ChangeActiveCanvas(dialogueUI as ICanvasController);
        EventManager.Instance.dialogueEvents.OnDialogueEnded += () => ChangeActiveCanvas(hudUI as ICanvasController);
        EventManager.Instance.dialogueEvents.OnNextDialogueLine += UpdateDialogueText;
        EventManager.Instance.dialogueEvents.OnDialogueChoicesEnabled += DisplayDialogueChoices;
        EventManager.Instance.dialogueEvents.OnDialogueChoicesDisabled += HideDialogueChoices;

        // Subscribing to NPC events
        EventManager.Instance.uiEvents.OnNPCInterfaceChangeRequest += UpdateDialogueBoxInterface;

        // Subscribing to InteractionPrompt events
        EventManager.Instance.uiEvents.OnDisplayInteractionPromptRequest += (requesterID, message) => hudUI.ShowInteractionPrompt(message);
        EventManager.Instance.uiEvents.OnHideInteractionPromptRequest += (requesterID, message) => hudUI.HideInteractionPrompt(message);

        // Subscribing to SceneEvents
        EventManager.Instance.sceneEvents.OnGameSceneLoaded += newGameScene => ChangeActiveCanvas(hudUI as ICanvasController);
        EventManager.Instance.sceneEvents.OnGameSceneUnloaded += oldGameScene => ChangeActiveCanvas(mainMenuUI as ICanvasController);

        // Notify EventManager that UIManager is listening
        EventManager.Instance.internalEvents.ManagerStartedListening(gameObject.name);
    }

    public void ChangeActiveCanvas(ICanvasController canvasController)
    {
        activeCanvas.CloseCanvas();
        canvasController.StartCanvas();

        activeCanvas = canvasController;

        EventManager.Instance.uiEvents.ActiveCanvasChanged(canvasController.canvasObject.name);
    }

    public void UpdateDialogueText(string nextLine) => dialogueUI.UpdateDialogueText(nextLine);
    public void DisplayDialogueChoices(List<string> options) => dialogueUI.DisplayDialogueChoices(options);
    public void HideDialogueChoices() => dialogueUI.HideDialogueChoices();
    public void UpdateDialogueBoxInterface(Sprite dialogueBox) => dialogueUI.UpdateDialogueBoxInterface(dialogueBox); 
    public void HandlePause() // Later:  Verify wich canvas in currently enabled and set pause menu based on that
    {
        if (pauseMenuCanvas.gameObject.activeSelf) ChangeActiveCanvas(hudUI as ICanvasController);
        else ChangeActiveCanvas(pauseUI as ICanvasController);
    }

    // Callback methods from buttons
    // MainMenu buttons
    public void OnNewGame() => mainMenuUI.OnNewGame();
    public void OnContinueGame() => mainMenuUI.OnContinueGame();
    public void OnOpenOptions() => mainMenuUI.OnOpenOptions();
    public void OnOpenCredits() => mainMenuUI.OnOpenCredits();
    public void OnOpenAbout() => mainMenuUI.OnOpenAbout();
    public void OnExitGame() => mainMenuUI.OnExitGame();

    // Ingame menu buttons
    public void OnReturnToGame() => pauseUI.OnReturnToGame();
    public void OnSave() => pauseUI.OnSave();
    public void OnOpenInventory() => pauseUI.OnOpenInventory();
    public void OnOpenQuests() => pauseUI.OnOpenQuests();
    public void OnOpenStates() => pauseUI.OnOpenStates();
    public void OnOpenMap() => pauseUI.OnOpenMap();
    //public void OnOpenOptions() => pauseUI.OnOpenOptions(); -> the ingame button will call the same method used by the mainMenu button
    public void OnExitToMenu() => pauseUI.OnExitToMenu();

}
