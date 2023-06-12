using UnityEngine;

/*
 * This class is the middle-man used for every communication between the different game systems
 */
public class EventManager : MonoBehaviour
{
    public static EventManager Instance; // This class is a singleton

    // Event classes <- These objects hold both the events themselves and the methods used to invoke them
    public InputEvents inputEvents { get; private set; }
    public DialogueEvents dialogueEvents { get; private set; }
    public NPCEvents npcEvents { get; private set; }
    public UIEvents uiEvents { get; set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("More than one EventSystem was found on your scene");
            Destroy(this);
        }

        Instance = this;

        // Initializing the Events classes
        inputEvents = new InputEvents();
        dialogueEvents = new DialogueEvents();
        npcEvents = new NPCEvents();
        uiEvents = new UIEvents();
    }

}
