using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

/*
 * This class wraps the Story object from Ink. 
 * This class does not implement any of the dialogue logic, it only interacts with the Ink system.
 */
public class InkStoryWrapper
{
    public TextAsset inkAsset { get; private set; }
    private Story inkStory;

    // Public accesible values, needed for the DialogueManager
    public bool canContinue => inkStory.canContinue;
    public int choicesCount => inkStory.currentChoices.Count;

    // Constructor
    public InkStoryWrapper(TextAsset inkAsset)
    {
        this.inkAsset = inkAsset;

        // We initialize the ink story only once because all the ink files are compiled into only one JSON
        inkStory = new Story(inkAsset.text);

        // Catching error on the ink file
        inkStory.onError += (msg, type) =>
        {
            if (type == Ink.ErrorType.Warning) Debug.LogWarning(msg);
            else Debug.LogError(msg);
        };
    }

    // Goes to the knot where this npc dialogue starts.
    // It can be used to change to any knot, if necessary.
    public void StartDialogueWith(string npcName)
    {
        inkStory.ChoosePathString(npcName); // The main knot of an npc dialogue must have the same name as the npc
    }

    // Returns the next dialogue line
    public string ContinueDialogue()
    {
        string dialogueLine = "";

        if (!inkStory.canContinue) Debug.LogWarning("Trying to continue a dialogue that has no new lines");
        else dialogueLine = inkStory.Continue();

        return dialogueLine;
    }

    // Returns a list of string containing all the choices the player can currently choose from
    public List<string> GetCurrentChoices()
    {
        if (inkStory.currentChoices.Count == 0) Debug.LogWarning("Trying to get choices in a dialogue that have no choices");

        List<string> currentChoices = new List<string>();

        if (inkStory.currentChoices.Count > 0)
        {
            foreach (Choice choice in inkStory.currentChoices) currentChoices.Add(choice.text);
        }

        return currentChoices;
    }

    // Makes a choice in the current dialogue.
    public void MakeChoice(int choiceIndex)
    {
        if (inkStory.currentChoices.Count == 0) Debug.LogWarning("Trying to make a choice in a dialogue that have no choices");

        inkStory.ChooseChoiceIndex(choiceIndex);
    }
}
