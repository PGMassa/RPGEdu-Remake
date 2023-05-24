/*
 * This class is responsible for managing every other class related to player controls.
 * The other player controller classes must not know or communicate with each other directly.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager), typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    // Other player controller classes
    private InputManager inputManager;
    private PlayerMovement playerMovement; 

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        playerMovement.MovePlayer(inputManager.directionalInput);
    }
}
