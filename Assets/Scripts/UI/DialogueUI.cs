using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/*
 * This class is responsible for controlling every UI component related to dialogue/interactable.
 * It decides which UI components to keep on the screen at each moment, and updates the value
 * of the text components associated with them.
 */
public class DialogueUI
{
    private TMP_Text interactablePrompt; // text used to show that an object/character is interactable

    private GameObject dialogueBox;
    private TMP_Text dialogueText;

    private List<GameObject> dialogueChoiceButtons; // buttons where the dialogue choices will presented to the player

    private List<string> promptTextStack; //In case more than one interactable object is requesting a prompt

    // Constructor
    public DialogueUI(TMP_Text interactablePrompt, GameObject dialogueBox, TMP_Text dialogueText, List<GameObject> dialogueChoiceButtons) 
    {
        this.interactablePrompt = interactablePrompt;
        this.dialogueBox = dialogueBox;
        this.dialogueText = dialogueText;
        this.dialogueChoiceButtons = dialogueChoiceButtons;

        promptTextStack = new List<string>();

        CleanDialogueUI();
    }

    // Sets dialogue UI to a clean state
    public void CleanDialogueUI()
    {
        interactablePrompt.text = "";
        interactablePrompt.gameObject.SetActive(true); // It should always be true, unless the dialogueBox is active

        dialogueText.text = "";
        HideDialogueChoices();
        dialogueBox.SetActive(false);
    }

    //InteractionPrompt-related methods
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
            // If the message to hide is not currently being displayed, then we only need to take it out of the "stack"
            promptTextStack.Remove(promptText);
        }
    }


    //DialogueBox-related methods
    public void StartDialogueUI(string firstLine = "")
    {
        interactablePrompt.gameObject.SetActive(false);

        dialogueText.text = firstLine;
        dialogueBox.SetActive(true);
    }

    public void UpdateDialogueText(string dialogueLine)
    {
        dialogueText.text = dialogueLine;
    }

    public void DisplayDialogueChoices(List<string> currentChoices)
    {
        // after testing, correct the case where currentChoices.Count > dialogueChoiceButtons.Count
        if (currentChoices.Count > dialogueChoiceButtons.Count)
        {
            Debug.LogWarning("The dialogue is trying to display more options than the UI system currently support");
        }

        for(int i = 0; i <= currentChoices.Count -1; i++)
        {
            dialogueChoiceButtons[i].GetComponentInChildren<TMP_Text>().text = currentChoices[i];
            dialogueChoiceButtons[i].SetActive(true);
            dialogueChoiceButtons[i].GetComponent<Button>().interactable = true;
            dialogueChoiceButtons[i].GetComponent<Button>().enabled = true;
        }

        // need to do this manually, because the FirstSelected option on the event system only works for the first time you enter on dialogue
        EventSystem.current.SetSelectedGameObject(dialogueChoiceButtons[0]);
    }

    public void HideDialogueChoices()
    {
        foreach(GameObject choiceButton in dialogueChoiceButtons)
        {
            choiceButton.GetComponentInChildren<TMP_Text>().text = "";
            choiceButton.GetComponent<Button>().interactable = false;
            choiceButton.GetComponent<Button>().enabled = false;
            choiceButton.gameObject.SetActive(false);
        }
    }

    public void CloseDialogueUI() 
    {
        interactablePrompt.gameObject.SetActive(true);

        dialogueText.text = "";
        dialogueBox.SetActive(false);
    }

}
