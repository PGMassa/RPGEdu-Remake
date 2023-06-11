using System;
using UnityEngine;
using UnityEngine.UI;

/*
 * This class is responsible for managing IInteractable objects and characters.
 * Any class that wants to register to interactable-related events, must do it here.
 */
public class InteractablesManager : MonoBehaviour
{
    public static InteractablesManager Instance; // this class is a singleton

    // "Interactablion prompts"-related events
    public event Action<string, string> OnDisplayInteractionPromptRequested; //parameters: what object triggered the interaction, and the message they are passing
    public event Action<string, string> OnHideInteractionPromptRequested;

    // NPC-related events
    public event Action<string, string> OnNPCDialogueRequested;
    public event Action<Sprite> OnNpcDialogueInterfaceChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one InteractablesManager component was found on this scene");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Called directly by IInteractable object or NPC
    public void DisplayInteractionPrompt (string applicant, string interactionPromptText)
    {
        OnDisplayInteractionPromptRequested?.Invoke(applicant, interactionPromptText);
    }

    // Called directly by IInteractable object or NPC
    public void HideInteractionPrompt(string applicant, string interactionPromptText)
    {
        OnHideInteractionPromptRequested?.Invoke(applicant, interactionPromptText);
    }

    // Called directly by IInteractable NPC
    public void TalkToNPC(string applicant, string characterName, Sprite customNPCDialogueBox = null )
    {
        if (customNPCDialogueBox != null) OnNpcDialogueInterfaceChanged?.Invoke(customNPCDialogueBox);
        OnNPCDialogueRequested?.Invoke(applicant, characterName);
    }
}
