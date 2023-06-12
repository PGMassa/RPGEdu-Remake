using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is responsible for controlling the flow of dialogue with an npc. 
 * This class does not interact with the Ink system directly, it only implements the dialogue logic.
 */
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; } // This class is a singleton

    [Header("Main Dialogue File")]
    [SerializeField] private TextAsset inkAsset;

    private InkStoryWrapper inkStoryWrapper;

    // Dialogue Events
    public event Action OnDialogueStarted;
    public event Action OnDialogueEnded;
    public event Action <string> OnNextDialogueLine;
    public event Action <List<string>>OnDialogueChoicesEnabled; // one frame after the dialogue reaches an choice
    public event Action OnDialogueChoicesDisabled; // one frame after the player makes a choice

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

    private void OnEnable()
    {
        // Doing it on a coroutine to avoid "execution order" shenanigans
        StartCoroutine(SubscribeCallbacks());
    }

    private IEnumerator SubscribeCallbacks()
    {
        // Subscribing Input-related callbacks
        yield return new WaitUntil(() => EventManager.Instance != null);
        EventManager.Instance.inputEvents.OnNextLine += ContinueDialogue;

        // Subscribing Interactable-related callbacks
        yield return new WaitUntil(() => InteractablesManager.Instance != null);
        InteractablesManager.Instance.OnNPCDialogueRequested += (applicant, npcName) => StartDialogueWith(npcName);

    }

    private void OnDisable()
    {
        // Unsubscribing Input-related callbacks
        EventManager.Instance.inputEvents.OnNextLine -= ContinueDialogue;

        // Subscribing Interactable-related callbacks
        InteractablesManager.Instance.OnNPCDialogueRequested -= (applicant, npcName) => StartDialogueWith(npcName);
    }

    private void StartDialogueWith(string npcName)
    {
        inkStoryWrapper.StartDialogueWith(npcName); // Changing Knot
        OnDialogueStarted?.Invoke();

        ContinueDialogue();
    }

    // Callback function -> called when the NextLine Action is performed.
    public void ContinueDialogue()
    {
        bool canContinue = inkStoryWrapper.canContinue;

        if (canContinue)
        {
            OnNextDialogueLine?.Invoke(inkStoryWrapper.ContinueDialogue());
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
        OnDialogueEnded?.Invoke();
    }

    private IEnumerator EnablePlayerChoices()
    {
        yield return new WaitForEndOfFrame();
        OnDialogueChoicesEnabled?.Invoke(inkStoryWrapper.GetCurrentChoices());
    }

    private IEnumerator DisablePlayerChoices()
    {
        yield return new WaitForEndOfFrame();
        OnDialogueChoicesDisabled?.Invoke();
    }

}
