using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// This interface should be implemented in any classe that want to receive the player input
public interface IInputProcessor
{
    void InteractAction();
}

/*
 * This class is responsible for enabling and disabling the InputSystem, as well as
 * changing ActionMaps(not yet implemented). It also receives and stores the inputs of each Action.
 */
public class InputManager : MonoBehaviour
{
    private PlayerInputs playerInputs;
    private List<IInputProcessor> subscribedInputProcessor; //keeps track of every object who wants to receive the player inputs

    // All the input values must have a public Get and a private Set
    public Vector2 directionalInput { get; private set; } //WASD input
    public bool isSprinting { get; private set; }

    private void OnEnable()
    {
        // Initializes playerInputs and set the apropriate callback methods to handle each Action
        if (playerInputs == null)
        {
            playerInputs = new PlayerInputs();

            playerInputs.PlayerControls.DirectionalMovement.performed += i => directionalInput = i.ReadValue<Vector2>();

            playerInputs.PlayerControls.Sprint.started += i => isSprinting = true;
            playerInputs.PlayerControls.Sprint.canceled += i => isSprinting = false;

            playerInputs.PlayerControls.Interact.performed += OnInteract;
        }

        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    private void Awake()
    {
        subscribedInputProcessor = new List<IInputProcessor>();
    }

    public void SubscribeInputProcessor(IInputProcessor inputProcessor) 
    {
        subscribedInputProcessor.Add(inputProcessor);
    }

    public void UnsubscribeInputProcessor(IInputProcessor inputProcessor)
    {
        subscribedInputProcessor.Remove(inputProcessor);
    }

    private void OnInteract(InputAction.CallbackContext action)
    {
        foreach (IInputProcessor inputProcessor in subscribedInputProcessor) inputProcessor.InteractAction();
    }

}