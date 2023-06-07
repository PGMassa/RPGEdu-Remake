using System;
using UnityEngine;

/* 
 * This singleton class is responsible for  enabling and disabling the InputSystem, as well as
 * changing ActionMaps. It also stores the player inputs and make them available 
 * for the other classes.
 */
public class InputManager : MonoBehaviour
{
    // Used when you want to change the current ActionMap
    public enum ActionMap
    {
        PlayerControls,
        DialogueUI,
        UI
    };

    [Header("Parameters")]
    [SerializeField] private ActionMap defaultActionMap;

    public static InputManager Instance { get; private set; } //this class is a singleton

    private PlayerInputs playerInputs; // reference to the InputSystem

    // Player movement -> PlayerControls ActionMap 
    public Vector2 directionalInput { get; private set; }
    public bool isSprinting { get; private set; }

    // Player interaction -> PlayerControls ActionMap
    public event Action OnPlayerInteraction;

    // Next dialogue line -> DialogueUI ActionMap
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
        }
    }

    private void OnEnable()
    {
        if(playerInputs == null)
        {
            playerInputs = new PlayerInputs();

            // PlayerControls callbacks
            playerInputs.PlayerControls.DirectionalMovement.performed += i => directionalInput = i.ReadValue<Vector2>();

            playerInputs.PlayerControls.Sprint.started += i => isSprinting = true;
            playerInputs.PlayerControls.Sprint.canceled += i => isSprinting = false;

            playerInputs.PlayerControls.Interact.performed += i => OnPlayerInteraction?.Invoke();

            // UIDialogue callbacks
            playerInputs.DialogueUI.NextLine.performed += i => OnNextLine?.Invoke();
        }

        SwapActionMap(defaultActionMap); //Enable the default actionMap
    }

    private void OnDisable()
    {
        playerInputs.Disable();
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

