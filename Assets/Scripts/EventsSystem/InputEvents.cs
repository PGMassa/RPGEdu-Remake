using System;
using UnityEngine;

public class InputEvents
{
    // Event declarations
    // ActionMap events
    public event Action<InputManager.ActionMap, InputManager.ActionMap> OnActionMapChanged; // Parameters: old ActionMap, new ActionMap

    // Player Controls events
    public event Action<Vector2> OnDirectionalInput;
    public event Action OnSprintingStarted;
    public event Action OnSprintingEnded;
    public event Action OnPlayerInteractionPerformed;

    // Dialogue Events
    public event Action OnNextLine;


    // Raising events
    // ActionMap events
    public void ActionMapChanged(InputManager.ActionMap oldActionMap, InputManager.ActionMap newActionMap) => OnActionMapChanged?.Invoke(oldActionMap, newActionMap);

    // Player Controls events
    public void DirectionalInput(Vector2 directionalInput) => OnDirectionalInput?.Invoke(directionalInput);
    public void SprintingStarted() => OnSprintingStarted?.Invoke();
    public void SprintingEnded() => OnSprintingEnded?.Invoke();
    public void PlayerInteractionPerformed() => OnPlayerInteractionPerformed?.Invoke();

    // Dialogue Events
    public void NextLine() => OnNextLine?.Invoke();


}
