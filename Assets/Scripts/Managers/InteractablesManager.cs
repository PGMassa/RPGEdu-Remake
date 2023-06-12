using System;
using UnityEngine;
using UnityEngine.UI;

/*
 * This class is responsible for managing IInteractable objects and characters.
 */
public class InteractablesManager : MonoBehaviour
{
    public static InteractablesManager Instance; // this class is a singleton

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
    public void DisplayInteractionPrompt (string requesterID, string message)
    {
        EventManager.Instance.uiEvents.DisplayInteractionPromptRequest(requesterID, message);
    }

    // Called directly by IInteractable object or NPC
    public void HideInteractionPrompt(string requesterID, string message)
    {
        EventManager.Instance.uiEvents.HideInteractionPromptRequest(requesterID, message);
    }

    // Called directly by IInteractable NPC
    public void TalkToNPC(string applicant, string characterName, Sprite customNPCDialogueBox = null )
    {
        if (customNPCDialogueBox != null) EventManager.Instance.uiEvents.NPCInterfaceChangeRequest(customNPCDialogueBox);
        EventManager.Instance.npcEvents.RequestNPCDialogue(characterName);
    }
}
