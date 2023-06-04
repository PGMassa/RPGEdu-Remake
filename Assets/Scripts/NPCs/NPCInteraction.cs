using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    [Header("NPC Info")]
    [SerializeField] string characterName;
    [SerializeField] string testDialogue;

    void IInteractable.DisplayInteractionPrompt()
    {
        UIManager.Instance.ShowInteractionPrompt("Pressione E para falar com " + characterName);
    }

    void IInteractable.HideInteractionPrompt()
    {
        UIManager.Instance.HideInterationPrompt("Pressione E para falar com " + characterName);
    }

    void IInteractable.Interact()
    {
        UIManager.Instance.StartDialogue(testDialogue);
    }

}
