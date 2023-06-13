using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * This class is responsible for tracking which managers and playercontroller have been initialized (and are ready to receive events) in a scene.
 */
public class InitializationTracker
{
    // Dictionaries used to track the initialization of the managers
    public Dictionary<string, bool> trackedManagers { get; private set; }

    // Used by the EventManager to determine when to broadcast the "managers ready" events
    private int initializedManagersCount;
    public bool areAllManagersListening => (initializedManagersCount - trackedManagers.Count()) == 0;

    // Constructor
    public InitializationTracker(List<GameObject> trackedManagers)
    {
        // Instantiating dictionaries
        this.trackedManagers = new Dictionary<string, bool>();

        // Populating dictionary of tracked managers
        foreach (GameObject manager in trackedManagers)
        {
            if (this.trackedManagers.ContainsKey(manager.name))
            {
                Debug.LogWarning("EventManager is set to track the same manager multiple times: " + manager.name);
                continue;
            }
            this.trackedManagers.Add(manager.name, false);
        }

        initializedManagersCount = 0;

        // Subscribing to internalEvents
        EventManager.Instance.internalEvents.OnManagerStartedListening += (managerName) => SetManagerInitializationTo(managerName, true);
        EventManager.Instance.internalEvents.OnManagerStoppedListening += (managerName) => SetManagerInitializationTo(managerName, false);
    }

    // Changes the initialization value of the Manager in the dictionary and update the count
    private void SetManagerInitializationTo(string managerName, bool isInitialized)
    {
        if (trackedManagers.ContainsKey(managerName))
        {
            trackedManagers[managerName] = isInitialized;
            initializedManagersCount += isInitialized ? 1 : -1;
        }
        else
        {
            Debug.LogWarning("Trying to change the initialization value of a manager that is not " +
                "currently being tracked by the EventManager: " + managerName);
        }
    }
}
