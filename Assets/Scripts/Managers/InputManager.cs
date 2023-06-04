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


    private void Awake()
    {
        if (Instance != null)
        {
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

            playerInputs.PlayerControls.DirectionalMovement.performed += i => directionalInput = i.ReadValue<Vector2>();

            playerInputs.PlayerControls.Sprint.started += i => isSprinting = true;
            playerInputs.PlayerControls.Sprint.canceled += i => isSprinting = false;

            playerInputs.PlayerControls.Interact.performed += i => OnPlayerInteraction?.Invoke();

        }

        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

}

