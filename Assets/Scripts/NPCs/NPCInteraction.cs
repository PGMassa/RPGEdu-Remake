using UnityEngine;
using UnityEngine.UI;

/*
 * This class is responsible for the IInteractables npcs
 * It tells the UI when to show the Interaction Prompt and 
 * tells the DialogueManager when to start a dialogue
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
        InteractablesManager.Instance.DisplayInteractionPrompt(gameObject.name, interactionPromptText);
    }

    void IInteractable.HideInteractionPrompt()
    {
        InteractablesManager.Instance.HideInteractionPrompt(gameObject.name, interactionPromptText);
    }

    void IInteractable.Interact()
    {
        InteractablesManager.Instance.TalkToNPC(gameObject.name, characterName, customNPCDialogueBox);
    }

}
