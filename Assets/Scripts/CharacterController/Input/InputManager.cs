/*
 * This class is responsible for enabling and disabling the InputSystem, as well as
 * changing ActionMaps(not yet implemented). It also receives and stores the inputs of each Action.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInputs playerInputs;

    // All the input values must have a public Get and a private Set
    public Vector2 directionalInput { get; private set; } //WASD input

    private void OnEnable()
    {
        // Initializes playerInputs and set the apropriate callback methods to handle each Action
        if (playerInputs == null)
        {
            playerInputs = new PlayerInputs();

            playerInputs.PlayerControls.DirectionalMovement.performed += i => directionalInput = i.ReadValue<Vector2>();
        }

        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

}
