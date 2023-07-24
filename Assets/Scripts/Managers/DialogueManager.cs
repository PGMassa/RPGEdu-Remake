using System.Collections;
using UnityEngine;

/*
 * This class is responsible for controlling the flow of dialogue with an npc. 
 * This class does not interact with the Ink system directly, it only implements the dialogue logic.
 */
public class DialogueManager : MonoBehaviour
{
    //public static DialogueManager Instance { get; private set; } // This class is a singleton

    [Header("Main Dialogue File")]
    [SerializeField] private TextAsset inkAsset;

    private InkStoryWrapper inkStoryWrapper;

    private void Awake()
    {
        inkStoryWrapper = new InkStoryWrapper(inkAsset);
    }

    private void OnEnable()
    {
        // Doing it on a coroutine to avoid "execution order" shenanigans
        StartCoroutine(SubscribeCallbacks());
    }

    private IEnumerator SubscribeCallbacks()
    {
        yield return new WaitUntil(() => EventManager.Instance != null);

        // Subscribing to Input evetns
        EventManager.Instance.inputEvents.OnNextLine += ContinueDialogue;

        // Subscribing to NPC events
        EventManager.Instance.npcEvents.OnNPCDialogueRequested += (npcID) => StartDialogueWith(npcID);

        // Notify EventManager that DialogueManager is listening
        EventManager.Instance.internalEvents.ManagerStartedListening(gameObject.name);

    }

    private void OnDisable()
    {
        // Unsubscribing to Input events
        EventManager.Instance.inputEvents.OnNextLine -= ContinueDialogue;

        // Unsubscribing to NPC events
        EventManager.Instance.npcEvents.OnNPCDialogueRequested -= (npcID) => StartDialogueWith(npcID);

        // Notify EventManager that DialogueManager is not listening anymore
        EventManager.Instance.internalEvents.ManagerStoppedListening(gameObject.name);
    }

    private void StartDialogueWith(string npcID)
    {
        inkStoryWrapper.StartDialogueWith(npcID); // Changing Knot
        EventManager.Instance.dialogueEvents.DialogueStarted();

        ContinueDialogue();
    }

    // Callback function -> called when the NextLine Action is performed.
    public void ContinueDialogue()
    {
        bool canContinue = inkStoryWrapper.canContinue;

        if (canContinue)
        {
            Debug.Log("Sistema de dialogos invocou a ação NextDialogueLine");
            EventManager.Instance.dialogueEvents.NextDialogueLine(inkStoryWrapper.ContinueDialogue());
        }

        if(inkStoryWrapper.choicesCount > 0)
        {
            StartCoroutine(EnablePlayerChoices());

        }else if (!canContinue) EndDialogue();

    }

    // Callback function -> called by the OnClick event of the choice buttons
    public void MakeChoice(int choiceIndex)
    {
        inkStoryWrapper.MakeChoice(choiceIndex);
        StartCoroutine(DisablePlayerChoices());

        ContinueDialogue();
    }

    private void EndDialogue()
    {
        EventManager.Instance.dialogueEvents.DialogueEnded();
    }

    private IEnumerator EnablePlayerChoices()
    {
        yield return new WaitForEndOfFrame();
        EventManager.Instance.dialogueEvents.DialogueChoicesEnabled(inkStoryWrapper.GetCurrentChoices());
    }

    private IEnumerator DisablePlayerChoices()
    {
        yield return new WaitForEndOfFrame();
        EventManager.Instance.dialogueEvents.DialogueChoicesDisabled();
    }

}
