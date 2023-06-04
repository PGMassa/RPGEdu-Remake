using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

/*
 * This class is responsible for managing everything related to dialogue/interactable object UI.
 * It decides which UI components to keep on the screen at each moment, and updates the value
 * of the text components associated with them.
 */
public class DialogueUI
{
    private TMP_Text interactablePrompt; // text used to show that an object/character is interactable

    private GameObject dialogueBox;
    private TMP_Text dialogueText;

    private List<string> promptTextStack; //In case more than one interactable object is requesting a prompt

    // Constructor
    public DialogueUI(TMP_Text interactablePrompt, GameObject dialogueBox, TMP_Text dialogueText) 
    {
        this.interactablePrompt = interactablePrompt;
        this.dialogueBox = dialogueBox;
        this.dialogueText = dialogueText;

        promptTextStack = new List<string>();

        CleanDialogueUI();
    }

    // Sets dialogue UI to a clean state
    public void CleanDialogueUI()
    {
        interactablePrompt.text = "";
        interactablePrompt.gameObject.SetActive(true); // It should always be true, unless the dialogueBox is active

        dialogueText.text = "";
        dialogueBox.SetActive(false);
    }

    public void ShowInteractionPrompt(string promptText)
    {
        // If some other object is already using the interaction prompt, store the old object's message
        // so we can recover it after the new object is done using the prompt
        if(!interactablePrompt.text.Equals("")) promptTextStack.Add(interactablePrompt.text);

        interactablePrompt.text = promptText;
    }

    public void HideInteractionPrompt(string promptText)
    {
        if (promptText.Equals(interactablePrompt.text)) 
        {
            // If the message to hide is the one currently being displayed, we need to get another message to replace it
            if (promptTextStack.Count == 0) interactablePrompt.text = "";
            else
            {
                interactablePrompt.text = promptTextStack.Last();
                promptTextStack.RemoveAt(promptTextStack.Count - 1);
            }
            
        }
        else
        {
            // If the message to hide is not currently being displayed, then we only need to take it out of the stack
            promptTextStack.Remove(promptText);
        }
    }

    public void StartDialogue(string firstLine = "")
    {
        interactablePrompt.gameObject.SetActive(false);

        dialogueText.text = firstLine;
        dialogueBox.SetActive(true);
    }

    public void EndDialogue() 
    {
        interactablePrompt.gameObject.SetActive(true);

        dialogueText.text = "";
        dialogueBox.SetActive(false);
    }



}
