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
    private Canvas dialogueCanvas;

    private GameObject dialogueBox;
    private TMP_Text dialogueText;

    private List<GameObject> dialogueChoiceButtons;

    // Constructor
    public DialogueUI(Canvas dialogueCanvas, GameObject dialogueBox, TMP_Text dialogueText, List<GameObject> dialogueChoiceButtons) 
    {
        this.dialogueCanvas = dialogueCanvas;
        this.dialogueBox = dialogueBox;
        this.dialogueText = dialogueText;
        this.dialogueChoiceButtons = dialogueChoiceButtons;

        CleanDialogueUI();
    }

    // Sets dialogue UI to a clean state
    public void CleanDialogueUI()
    {
        dialogueText.text = "";
        HideDialogueChoices();
        dialogueBox.SetActive(false);
    }

    //DialogueBox-related methods
    public void StartDialogueUI()
    {
        dialogueCanvas.gameObject.SetActive(true);

        dialogueText.text = "";
        dialogueBox.SetActive(true);
    }

    public void UpdateDialogueText(string dialogueLine)
    {
        dialogueText.text = dialogueLine;
    }

    public void UpdateDialogueBoxInterface(Sprite dialogueBoxImage) 
    {
        dialogueBox.GetComponent<Image>().sprite = dialogueBoxImage;
    }

    public void DisplayDialogueChoices(List<string> currentChoices)
    {
        int choicesCount =
            (currentChoices.Count > dialogueChoiceButtons.Count) ? 
            dialogueChoiceButtons.Count : currentChoices.Count; // how many choice button to display

        if (currentChoices.Count > dialogueChoiceButtons.Count)
        {
            Debug.LogWarning("The dialogue is trying to display more options than the UI system currently support");
        }

        for(int i = 0; i <= choicesCount -1; i++)
        {
            dialogueChoiceButtons[i].GetComponentInChildren<TMP_Text>().text = currentChoices[i];
            dialogueChoiceButtons[i].SetActive(true);
            dialogueChoiceButtons[i].GetComponent<Button>().interactable = true;
            dialogueChoiceButtons[i].GetComponent<Button>().enabled = true;
        }

        // Yes, we need to do this manually. The "FirstSelected" option on the event system only works for the first time you enter on dialogue
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
        dialogueCanvas.gameObject.SetActive(false);
        //interactablePrompt.gameObject.SetActive(true);

        dialogueText.text = "";
        dialogueBox.SetActive(false);
    }

}
