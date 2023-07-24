using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/*
 * This class is responsible for controlling every UI component related to the HUD.
 * It decides which UI components to keep on the screen at each moment, and updates the value
 * of the text components associated with them.
 */

public class HUDUI
{
    private Canvas hudCanvas;
    private TMP_Text interactablePrompt; // Text used to show that an object/character is interactable

    private List<string> promptTextStack; // In case more than one interactable object is requesting a prompt

    public HUDUI (Canvas hudCanvas, TMP_Text interactablePrompt)
    {
        this.hudCanvas = hudCanvas;
        this.interactablePrompt = interactablePrompt;

        promptTextStack = new List<string>();

        CleanHUDUI();
    }

    public void StartHUDUI()
    {
        hudCanvas.gameObject.SetActive(true);
        interactablePrompt.gameObject.SetActive(true);
    }

    public void CloseHUDUI()
    {
        hudCanvas.gameObject.SetActive(false);
        interactablePrompt.gameObject.SetActive(false);
    }

    // Sets HUD UI to a clean state
    public void CleanHUDUI()
    {
        interactablePrompt.text = "";
        interactablePrompt.gameObject.SetActive(true);
    }

    public void ShowInteractionPrompt(string promptText)
    {
        Debug.Log("SHOW INTERACTION PROMPT");
        interactablePrompt.gameObject.SetActive(true);
        // If some other object is already using the interaction prompt, store the old object's message
        // so we can recover it after the new object is done using the prompt
        if (!interactablePrompt.text.Equals("")) promptTextStack.Add(interactablePrompt.text);

        interactablePrompt.text = promptText;
    }

    public void HideInteractionPrompt(string promptText)
    {
        interactablePrompt.gameObject.SetActive(false);
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


}
