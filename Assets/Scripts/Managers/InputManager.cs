using System;
using UnityEngine;

/* 
 * This singleton class is responsible for  enabling and disabling the InputSystem, as well as
 * changing ActionMaps(not yet implemented). It also stores the player inputs and make them available 
 * for the other classes.
 */
public class InputManager : MonoBehaviour
{
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

        playerInputs.Enable();
        EnablePlayerControls(); //default action map
        playerInputs.UI.Enable(); //UI always enabled? <- test it later
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    // Change current ActionMap to DialogueUI
    public void EnableDialogueUI()
    {
        playerInputs.PlayerControls.Disable();
        playerInputs.DialogueUI.Enable();
        playerInputs.UI.Disable();
    }

    // Change current ActionMap to PlayerControls
    public void EnablePlayerControls()
    {
        playerInputs.DialogueUI.Disable();
        playerInputs.PlayerControls.Enable();
        playerInputs.UI.Disable();
    }

    public void EnableUI()
    {
        playerInputs.DialogueUI.Disable();
        playerInputs.PlayerControls.Disable();
        playerInputs.UI.Enable();
    }

}

