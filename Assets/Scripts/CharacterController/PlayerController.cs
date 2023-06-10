using System.Collections;
using UnityEngine;

/*
 * This class is responsible for managing player-related classes
 */
[RequireComponent(typeof(PlayerMovement), typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerInteraction))]
public class PlayerController : MonoBehaviour
{
    // Other player controller classes
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;
    private PlayerInteraction playerInteraction;

    // Movement variables
    private Vector2 directionalInput;
    private bool isSprinting;

    private void Start()
    {
        directionalInput = Vector2.zero;

        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    private void OnEnable()
    {
        // Doing it on a coroutine to avoid "execution order" shenanigans
        StartCoroutine(SubscribeCallbacks());
    }

    private IEnumerator SubscribeCallbacks()
    {
        // Subscribing Movement-related callbacks
        yield return new WaitUntil(() => InputManager.Instance != null);

        InputManager.Instance.OnDirectionInput += i => directionalInput = i;
        InputManager.Instance.OnSprintingStarted += () => isSprinting = true;
        InputManager.Instance.OnSprintingEnded += () => isSprinting = false;
        InputManager.Instance.OnPlayerInteraction += Interact;
    }

    private void OnDisable()
    {
        // Unsubscribing Movement-related callbacks
        InputManager.Instance.OnDirectionInput += i => directionalInput = i;
        InputManager.Instance.OnSprintingStarted += () => isSprinting = true;
        InputManager.Instance.OnSprintingEnded += () => isSprinting = false;
        InputManager.Instance.OnPlayerInteraction += Interact;
    }

    private void FixedUpdate()
    {
        playerMovement.HandleMovement(directionalInput, isSprinting);
        playerAnimation.HandleWalkingAnimation(directionalInput, isSprinting);
    }

    // Callback function -> called when the Interact Action is performed.
    public void Interact()
    {
        playerInteraction.Interact();
    }
}
