using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is the middle-man used for every communication between the different game systems
 */
public class EventManager : MonoBehaviour
{
    public static EventManager Instance; // This class is a singleton

    // Tracked objects are used check if every relevant manager have been fully initalized in a scene
    [Header("Tracked Game Managers")]
    [SerializeField]private List<GameObject> trackedManagers;

    private InitializationTracker initializationTracker; // Keeps track of initialization the tracked objects


    // Event classes <- These objects hold both the events themselves and the methods used to invoke them
    public InternalEvents internalEvents { get; private set; } // events related to the EventManager itself
    public PlayerEvents playerEvents { get; private set; }
    public InputEvents inputEvents { get; private set; }
    public DialogueEvents dialogueEvents { get; private set; }
    public NPCEvents npcEvents { get; private set; }
    public UIEvents uiEvents { get; set; }
    public SavingAndLoadingEvents savingAndLoadingEvents { get; set; }
    public SceneEvents sceneEvents { get; set; }


    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("More than one EventSystem was found on your scene");
            Destroy(this);
        }

        // Initializing the Events classes
        internalEvents = new InternalEvents();
        playerEvents = new PlayerEvents();
        inputEvents = new InputEvents();
        dialogueEvents = new DialogueEvents();
        npcEvents = new NPCEvents();
        uiEvents = new UIEvents();
        savingAndLoadingEvents = new SavingAndLoadingEvents();
        sceneEvents = new SceneEvents();

        Instance = this;
        initializationTracker = new InitializationTracker(trackedManagers);
    }

    private void OnEnable()
    {
        // When a new scene is loaded, wait until every necessary manager/controller is ready and broadcast a event about it
        StartCoroutine(BroadcastReadyState());
    }

    // Broadcast an event when every necessary manager and controller subscribed to their respective events
    private IEnumerator BroadcastReadyState()
    {
        // Wait until all the managers are ready
        yield return new WaitUntil(() => initializationTracker.areAllManagersListening == true);
        Debug.Log("All the managers are ready");

        // Broadcast event here
        Instance.internalEvents.AllManagersReady();
    }

}
