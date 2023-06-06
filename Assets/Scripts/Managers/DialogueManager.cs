using System.Collections;
using UnityEngine;

/*
 * This class is responsible for controlling the flow of dialogue with an npc. 
 * This class does not interact with the Ink system directly, it only implements the dialogue logic.
 * 
 * This class also informs other classes about changes necessary to the UI, 
 * ActionMaps, etc. (change this to use to events later).
 */

/*
 * TODO: 
 * Task1: Check the original game, and then change the choice buttons UI to try reproducing the original look as closely as possible
 *      
 * Task2: Threat the case where there are more choices than buttons
 * 
 * Task3: Add the actions of the UIDialogue actionmap to the UI actionmap an implement a better way to enable/disable the choices action
 * 
 * Task3: Refactor the dialogue code so it used events instead of direct calls to methods
 * 
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

    // Set the wrapper to dialogue with a given npc
    public void StartDialogueWith(string npcName)
    {
        inkStoryWrapper.StartDialogueWith(npcName);

        // --> Remember to change it to Events later, after testing <--
        UIManager.Instance.StartDialogueUI();
        InputManager.Instance.EnableDialogueUI();

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

    // This method is called by the OnClick event on the choice buttons
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
        InputManager.Instance.EnablePlayerControls();
    }

    private IEnumerator EnablePlayerChoices()
    {
        yield return new WaitForEndOfFrame();

        UIManager.Instance.DisplayDialogueChoices(inkStoryWrapper.GetCurrentChoices());
        InputManager.Instance.EnableUI();
    }

    private IEnumerator DisablePlayerChoices()
    {
        yield return new WaitForEndOfFrame();

        // --> Remember to change it to Events later, after testing <--
        UIManager.Instance.HideDialogueChoices();

        InputManager.Instance.EnableDialogueUI();
    }

}
