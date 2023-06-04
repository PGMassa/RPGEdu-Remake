using UnityEngine;

/*
 * This class is responsible for managing every other class related to player controls.
 * The other player controller classes must not know or communicate with each other directly.
 */
[RequireComponent(typeof(PlayerMovement), typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerInteraction))]
public class PlayerController : MonoBehaviour
{
    // Other player controller classes
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;
    private PlayerInteraction playerInteraction;

    private void Start()
    {
        Cursor.visible = false; // here temporarily - move to a interface manager once it's implemented

        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerInteraction = GetComponent<PlayerInteraction>();

        InputManager.Instance.OnPlayerInteraction += Interact;
    }

    private void FixedUpdate()
    {
        Vector2 directionalInput = InputManager.Instance.directionalInput;
        bool isSprinting = InputManager.Instance.isSprinting;

        playerMovement.HandleMovement(directionalInput, isSprinting);
        playerAnimation.HandleWalkingAnimation(directionalInput, isSprinting);
    }

    // This method is subscribed to an event on InputManager, and will be called when an InteractAction is performed by the player
    public void Interact()
    {
        playerInteraction.Interact();
    }
}
