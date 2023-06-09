using System.Collections;
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

    public void StartDialogueWith(string npcName)
    {
        inkStoryWrapper.StartDialogueWith(npcName);

        // --> Remember to change it to Events later, after testing <--
        UIManager.Instance.StartDialogueUI();
        InputManager.Instance.SwapActionMap(InputManager.ActionMap.DialogueUI);

        ContinueDialogue();
    }

    // This method is called when the NextLine Action is performed. 
    // It gets the new dialogue options/dialogue lines from the InkStoryWrapper and send them to the UI
    public void ContinueDialogue()
    {
        if (inkStoryWrapper.canContinue)
        {
            UIManager.Instance.UpdateDialogueText(inkStoryWrapper.ContinueDialogue());
        }

        if(inkStoryWrapper.choicesCount > 0)
        {
            StartCoroutine(EnablePlayerChoices());

        }else if (!inkStoryWrapper.canContinue) EndDialogue();

    }

    // This method is called by the OnClick event, on the choice buttons
    public void MakeChoice(int choiceIndex)
    {
        inkStoryWrapper.MakeChoice(choiceIndex);
        StartCoroutine(DisablePlayerChoices());

        ContinueDialogue();
    }

    private void EndDialogue()
    {
        // --> Remember to change it to Events later, after testing <--
        UIManager.Instance.CloseDialogueUI();
        InputManager.Instance.SwapActionMap(InputManager.ActionMap.PlayerControls);
    }

    private IEnumerator EnablePlayerChoices()
    {
        yield return new WaitForEndOfFrame();

        // --> Remember to change it to Events later, after testing <--
        UIManager.Instance.DisplayDialogueChoices(inkStoryWrapper.GetCurrentChoices());
        InputManager.Instance.SwapActionMap(InputManager.ActionMap.UI);
    }

    private IEnumerator DisablePlayerChoices()
    {
        yield return new WaitForEndOfFrame();

        // --> Remember to change it to Events later, after testing <--
        UIManager.Instance.HideDialogueChoices();
        InputManager.Instance.SwapActionMap(InputManager.ActionMap.DialogueUI);
    }

}
