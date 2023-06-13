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
        yield return new WaitUntil(() => EventManager.Instance != null);

        EventManager.Instance.inputEvents.OnDirectionalInput += i => directionalInput = i;
        EventManager.Instance.inputEvents.OnSprintingStarted += () => isSprinting = true;
        EventManager.Instance.inputEvents.OnSprintingEnded += () => isSprinting = false;
        EventManager.Instance.inputEvents.OnPlayerInteractionPerformed += Interact;

        // Subscribing callback for changes to the enabled ActionMap
        EventManager.Instance.inputEvents.OnActionMapChanged += (oldActionMap, newActionMap) => directionalInput = Vector2.zero;
    }

    private void OnDisable()
    {
        // Unsubscribing Movement-related callbacks
        EventManager.Instance.inputEvents.OnDirectionalInput -= i => directionalInput = i;
        EventManager.Instance.inputEvents.OnSprintingStarted -= () => isSprinting = true;
        EventManager.Instance.inputEvents.OnSprintingEnded -= () => isSprinting = false;
        EventManager.Instance.inputEvents.OnPlayerInteractionPerformed -= Interact;

        // Unubscribing callback for changes to the enabled ActionMap
        EventManager.Instance.inputEvents.OnActionMapChanged -= (oldActionMap, newActionMap) => directionalInput = Vector2.zero;
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
