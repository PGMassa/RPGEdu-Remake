using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is responsible for managing every other class related to player controls.
 * The other player controller classes must not know or communicate with each other directly.
 */
[RequireComponent(typeof(InputManager), typeof(PlayerMovement), typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerInteraction))]
public class PlayerController : MonoBehaviour
{
    private PlayerControllerInterface playerControllerInterface;

    // Other player controller classes
    private InputManager inputManager;
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;
    private PlayerInteraction playerInteraction;

    private void Awake()
    {
        Cursor.visible = false; // here temporarily - move to a interface manager once it's implemented

        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    private void Start()
    {
        //Instantiate the interface and subscribe it to receive player inputs
        playerControllerInterface = new PlayerControllerInterface(this);
        inputManager.SubscribeInputProcessor(playerControllerInterface);
    }

    private void FixedUpdate()
    {
        Vector2 directionalInput = inputManager.directionalInput;
        bool isSprinting = inputManager.isSprinting;

        playerMovement.HandleMovement(directionalInput, isSprinting);
        playerAnimation.HandleWalkingAnimation(directionalInput, isSprinting);
    }

    // This method is called by PlayerControllerInterface whenever it receives an InputAction
    public void Interact()
    {
        playerInteraction.Interact();
    }
}


/*
 * This class is responsible for playerController communications. Any class that wants
 * to communicate with it, can do throgh this class.
 */
public class PlayerControllerInterface : IInputProcessor
{
    private PlayerController playerController;

    public PlayerControllerInterface(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    void IInputProcessor.InteractAction()
    {
        playerController.Interact();
    }
}