using System;

/*
 * This class is responsible for any event related to the EventManager itself
 */
public class InternalEvents
{
    // Event declarations
    public event Action<string> OnManagerStartedListening;
    public event Action<string> OnManagerStoppedListening;

    public event Action OnAllManagersReady;

    // Raising events
    public void ManagerStartedListening(string managerName) => OnManagerStartedListening?.Invoke(managerName);
    public void ManagerStoppedListening(string managerName) => OnManagerStoppedListening?.Invoke(managerName);

    public void AllManagersReady() => OnAllManagersReady?.Invoke();
}
