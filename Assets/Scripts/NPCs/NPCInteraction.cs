using UnityEngine;

/*
 * This class is responsible for the IInteractables npcs
 * It tells the UI when to show the Interaction Prompt and 
 * tells the DialogueManager when to start a dialogue
 */
public class NPCInteraction : MonoBehaviour, IInteractable
{
    [Header("NPC Info")]
    [SerializeField] private string characterName;

    private string interactionPromptText;

    void Awake()
    {
        interactionPromptText = "Pressione E parar falar com " + characterName;
    }

    void IInteractable.DisplayInteractionPrompt()
    {
        UIManager.Instance.ShowInteractionPrompt(interactionPromptText);
    }

    void IInteractable.HideInteractionPrompt()
    {
        UIManager.Instance.HideInterationPrompt(interactionPromptText);
    }

    void IInteractable.Interact()
    {
        DialogueManager.Instance.StartDialogueWith(characterName);
    }

}
