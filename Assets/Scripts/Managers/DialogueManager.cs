using System.Collections.Generic;
using UnityEngine;

/*
 * This class is responsible for controlling the flow of dialogue with an npc. 
 * This class does not interact with the Ink system directly, it only implements the dialogue logic.
 * 
 * This class also informs other classes about changes necessary to the UI, 
 * ActionMaps, etc. (change this to use to events later).
 */
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; } // This class is a singleton

    [Header("Main Dialogue File")]
    [SerializeField] private TextAsset inkAsset;

    private InkStoryWrapper inkStoryWrapper;

    private Queue<string> dialogueLinesQueue;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("More than one DialogueManager component was found on this scene");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

            inkStoryWrapper = new InkStoryWrapper(inkAsset);
        }
    }

    private void Start()
    {
        InputManager.Instance.OnNextLine += ContinueDialogue;
    }

    // Set the wrapper to dialogue with a given npc
    public void StartDialogueWith(string npcName)
    {
        inkStoryWrapper.StartDialogueWith(npcName);
        dialogueLinesQueue = new Queue<string>();

        // --> Remember to change it to Events later, after testing <--
        UIManager.Instance.StartDialogueUI();
        InputManager.Instance.EnableDialogueUI();

        ContinueDialogue();
    }

    // This method is called when the NextLine Action is performed. 
    // It gets the new dialogue options/dialogue lines from the InkStoryWrapper and send them to the UI
    public void ContinueDialogue()
    {
        if (dialogueLinesQueue.Count != 0)
        {
            // Send next line of dialogue to the UI manager
            // --> Remember to change it to Events later, after testing <--
            UIManager.Instance.UpdateDialogueText(dialogueLinesQueue.Dequeue());
            return;
        }

        if (inkStoryWrapper.canContinue)
        {
            // get new lines of dialogue and send the first to the UI
            dialogueLinesQueue = new Queue<string>(inkStoryWrapper.ContinueDialogue());
            // --> Remember to change it to Events later, after testing <--
            UIManager.Instance.UpdateDialogueText(dialogueLinesQueue.Dequeue());
            return;
        }

        if (inkStoryWrapper.choicesCount > 0)
        {
            // gets current choices and send them to the UI
            List<string> currentChoices = inkStoryWrapper.GetCurrentChoices();
            // --> Remember to change it to Events later, after testing <--
            Debug.Log("UI manager does not implement choices yet. Implement it before calling this method");
            return;
        }

        EndDialogue();
    }

    private void EndDialogue()
    {
        // --> Remember to change it to Events later, after testing <--
        UIManager.Instance.CloseDialogueUI();
        InputManager.Instance.EnablePlayerControls();
    }

}
