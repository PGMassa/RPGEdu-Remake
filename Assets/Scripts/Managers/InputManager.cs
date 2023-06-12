using System.Collections;
using UnityEngine;

/* 
 * This class is responsible for managing the Unity InputSystem
 */
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; } //this class is a singleton

    public enum ActionMap //Used to swap ActionMap
    {
        PlayerControls,
        DialogueUI,
        UI,
        None
    };

    [Header("Parameters")]
    [SerializeField] private ActionMap defaultActionMap;

    private PlayerInputs playerInputs; // reference to the InputSystem

    private ActionMap enabledActionMap;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one InputManager component was found on this scene");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

            playerInputs = new PlayerInputs();
            enabledActionMap = ActionMap.None;
        }
    }

    private void OnEnable()
    {
        // Enable the default ActionMap (after all the important Managers have been initialized)
        StartCoroutine(EnableDefaultActionMap());

        // Doing it on a coroutine to avoid "execution order" shenanigans
        StartCoroutine(SubscribeCallbacks());
    }

    private IEnumerator EnableDefaultActionMap()
    {
        // !!! Move this logic to the EventManager later !!!

        // Waiting until all the important Instances were initialized before enabling playerInput
        yield return new WaitUntil(() => DialogueManager.Instance != null);
        yield return new WaitUntil(() => UIManager.Instance != null);
        yield return new WaitUntil(() => InteractablesManager.Instance != null);

        SwapActionMap(defaultActionMap);
    }

    // Doing it on a coroutine to avoid "execution order" shenanigans
    private IEnumerator SubscribeCallbacks()
    {
        yield return new WaitUntil(() => EventManager.Instance != null);
        
        // Subscribing PlayerControls events
        playerInputs.PlayerControls.DirectionalMovement.performed += i => EventManager.Instance.inputEvents.DirectionalInput(i.ReadValue<Vector2>());
        playerInputs.PlayerControls.Sprint.started += i => EventManager.Instance.inputEvents.SprintingStarted();
        playerInputs.PlayerControls.Sprint.canceled += i => EventManager.Instance.inputEvents.SprintingEnded();
        playerInputs.PlayerControls.Interact.performed += i => EventManager.Instance.inputEvents.PlayerInteractionPerformed();

        // Subscribing DialogueUI events
        playerInputs.DialogueUI.NextLine.performed += i => EventManager.Instance.inputEvents.NextLine();

        // Subscribing to ActionMap-changing events
        EventManager.Instance.dialogueEvents.OnDialogueStarted += () => SwapActionMap(ActionMap.DialogueUI);
        EventManager.Instance.dialogueEvents.OnDialogueEnded += () => SwapActionMap(ActionMap.PlayerControls);
        EventManager.Instance.dialogueEvents.OnDialogueChoicesEnabled += i => SwapActionMap(ActionMap.UI);
        EventManager.Instance.dialogueEvents.OnDialogueChoicesDisabled += () => SwapActionMap(ActionMap.DialogueUI);

    }

    private void OnDisable()
    {
        // Unsubscribing PlayerControls events
        playerInputs.PlayerControls.DirectionalMovement.performed -= i => EventManager.Instance.inputEvents.DirectionalInput(i.ReadValue<Vector2>());
        playerInputs.PlayerControls.Sprint.started -= i => EventManager.Instance.inputEvents.SprintingStarted();
        playerInputs.PlayerControls.Sprint.canceled -= i => EventManager.Instance.inputEvents.SprintingEnded();
        playerInputs.PlayerControls.Interact.performed -= i => EventManager.Instance.inputEvents.PlayerInteractionPerformed();

        // Unsubscribing DialogueUI" events
        playerInputs.DialogueUI.NextLine.performed -= i => EventManager.Instance.inputEvents.NextLine();

        // Unsubscribing to ActionMap-changing events
        EventManager.Instance.dialogueEvents.OnDialogueStarted -= () => SwapActionMap(ActionMap.DialogueUI);
        EventManager.Instance.dialogueEvents.OnDialogueEnded -= () => SwapActionMap(ActionMap.PlayerControls);
        EventManager.Instance.dialogueEvents.OnDialogueChoicesEnabled -= i => SwapActionMap(ActionMap.UI);
        EventManager.Instance.dialogueEvents.OnDialogueChoicesDisabled -= () => SwapActionMap(ActionMap.DialogueUI);


        playerInputs.Disable(); // Disable all ActionMaps
    }

    public void SwapActionMap(ActionMap actionMap)
    {
        playerInputs.Disable();
        switch (actionMap)
        {
            case ActionMap.PlayerControls:
                playerInputs.PlayerControls.Enable();
                break;
            case ActionMap.DialogueUI:
                playerInputs.DialogueUI.Enable();
                break;
            case ActionMap.UI:
                playerInputs.UI.Enable();
                break;
            default:
                Debug.LogError("Trying to swap current ActionMap to an ActionMap that is not currently implemented");
                break;
        }

        Debug.Log("Enabled ActionMap: " + actionMap);
        EventManager.Instance.inputEvents.ActionMapChanged(enabledActionMap, actionMap);
        enabledActionMap = actionMap;
    }
}

