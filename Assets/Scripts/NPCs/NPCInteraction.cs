using System.Collections;
using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] string CharacterName;

    void IInteractable.DisplayInteractionPrompt()
    {
        UIManager.Instance.ShowInteractablePromptText("Pressione E para falar com " + CharacterName);
    }

    void IInteractable.HideInteractionPrompt()
    {
        UIManager.Instance.HideInteratablePromptText();
    }

    void IInteractable.Interact()
    {
        UIManager.Instance.ShowDialogueBox("Hello there");
    }

}
