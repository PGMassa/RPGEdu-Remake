using UnityEngine;
using UnityEngine.UI;

/*
 * Implementation of the IInteractable interface for NPCs
 */
public class NPCInteraction : MonoBehaviour, IInteractable
{
    [Header("NPC Info")]
    [SerializeField] private string characterName;

    [Header("NPC Dialogue Interface")]
    [SerializeField] private Sprite customNPCDialogueBox; // Later this will have to be changed, once we get new npcs.

    private string interactionPromptText;

    void Awake()
    {
        interactionPromptText = "Pressione E parar falar com " + characterName;
    }

    void IInteractable.DisplayInteractionPrompt()
    {
        EventManager.Instance.uiEvents.DisplayInteractionPromptRequest(characterName, interactionPromptText);
    }

    void IInteractable.HideInteractionPrompt()
    {
        EventManager.Instance.uiEvents.HideInteractionPromptRequest(characterName, interactionPromptText);
    }

    void IInteractable.Interact()
    {
        if (customNPCDialogueBox == null) Debug.LogWarning("No customDialogueBox ascribed to the npc: " + characterName);
        else EventManager.Instance.uiEvents.NPCInterfaceChangeRequest(customNPCDialogueBox);
       
        EventManager.Instance.npcEvents.RequestNPCDialogue(characterName);
    }

}
