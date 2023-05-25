/*
 * This class is responsible for managing every other class related to player controls.
 * The other player controller classes must not know or communicate with each other directly.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager), typeof(PlayerMovement), typeof(PlayerAnimation))]
public class PlayerController : MonoBehaviour
{
    // Other player controller classes
    private InputManager inputManager;
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void FixedUpdate()
    {
        Vector2 directionalInput = inputManager.directionalInput;
        bool isSprinting = inputManager.isSprinting;

        playerMovement.HandleMovement(directionalInput, isSprinting);
        playerAnimation.HandleWalkingAnimation(directionalInput, isSprinting);
    }
}
