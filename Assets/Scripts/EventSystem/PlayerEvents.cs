using System;
using UnityEngine;

public class PlayerEvents
{
    // Event declarations
    public event Action<InputManager.ActionMap> OnPlayerControllerEnabled;
    public event Action OnPlayerControllerDisabled;

    // Raising events
    public void PlayerControllerEnabled(InputManager.ActionMap playerActionMap) => OnPlayerControllerEnabled?.Invoke(playerActionMap);
    public void PlayerControllerDisabled() => OnPlayerControllerDisabled?.Invoke();
}
