using System;
using System.Collections;
using UnityEngine;

/* 
 * This singleton class is responsible for  enabling and disabling the InputSystem, as well as
 * changing ActionMaps. It also stores the player inputs and make them available 
 * for the other classes.
 */
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; } //this class is a singleton

    public enum ActionMap //Used to swap ActionMap
    {
        PlayerControls,
        DialogueUI,
        UI
    };

    [Header("Parameters")]
    [SerializeField] private ActionMap defaultActionMap;

    private PlayerInputs playerInputs; // reference to the InputSystem

    // Player Controls Events
    public event Action <Vector2> OnDirectionInput;
    public event Action OnSprintingStarted;
    public event Action OnSprintingEnded;
    public event Action OnPlayerInteraction;

    // Dialogue Events
    public event Action OnNextLine;

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
        }
    }

    private void OnEnable()
    {
        /*
         * There is no need to put the InputSystem-related subscriptions inside a coroutine, since we are instantianted
         * playerInput ourselves.
         * 
         * Subscriptions to other events, however, will be put inside the "SubscribeCallbacks" coroutine, as usual
         */

        // Subscribing to "PlayerControls" events
        playerInputs.PlayerControls.DirectionalMovement.performed += i => OnDirectionInput?.Invoke(i.ReadValue<Vector2>());
        playerInputs.PlayerControls.Sprint.started += i => OnSprintingStarted?.Invoke();
        playerInputs.PlayerControls.Sprint.canceled += i => OnSprintingEnded?.Invoke();
        playerInputs.PlayerControls.Interact.performed += i => OnPlayerInteraction?.Invoke();

        // Subscribing to "UIDialogue" events
        playerInputs.DialogueUI.NextLine.performed += i => OnNextLine?.Invoke();

        // Enable the default ActionMap
        SwapActionMap(defaultActionMap);

        // Doing it on a coroutine to avoid "execution order" shenanigans
        StartCoroutine(SubscribeCallbacks());
    }

    // Doing it on a coroutine to avoid "execution order" shenanigans
    private IEnumerator SubscribeCallbacks()
    {
        // Subscribing callbacks related to the swapping of ActionMaps
        yield return new WaitUntil(() => DialogueManager.Instance != null);

        DialogueManager.Instance.OnDialogueStarted += () => SwapActionMap(ActionMap.DialogueUI);
        DialogueManager.Instance.OnDialogueEnded += () => SwapActionMap(ActionMap.PlayerControls);
        DialogueManager.Instance.OnDialogueChoicesEnabled += i => SwapActionMap(ActionMap.UI);
        DialogueManager.Instance.OnDialogueChoicesDisabled += () => SwapActionMap(ActionMap.DialogueUI);

    }

    private void OnDisable()
    {
        // Unubscribing to the InputSystem events
        // PlayerControls callbacks
        playerInputs.PlayerControls.DirectionalMovement.performed -= i => OnDirectionInput?.Invoke(i.ReadValue<Vector2>());
        playerInputs.PlayerControls.Sprint.started -= i => OnSprintingStarted?.Invoke();
        playerInputs.PlayerControls.Sprint.canceled -= i => OnSprintingEnded?.Invoke();
        playerInputs.PlayerControls.Interact.performed -= i => OnPlayerInteraction?.Invoke();

        // UIDialogue callbacks
        playerInputs.DialogueUI.NextLine.performed -= i => OnNextLine?.Invoke();

        // Unsubscribing callbakcs related to the swapping of ActionMaps
        DialogueManager.Instance.OnDialogueStarted -= () => SwapActionMap(ActionMap.DialogueUI);
        DialogueManager.Instance.OnDialogueEnded -= () => SwapActionMap(ActionMap.PlayerControls);
        DialogueManager.Instance.OnDialogueChoicesEnabled -= i => SwapActionMap(ActionMap.UI);
        DialogueManager.Instance.OnDialogueChoicesDisabled -= () => SwapActionMap(ActionMap.DialogueUI);


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
    }
}

