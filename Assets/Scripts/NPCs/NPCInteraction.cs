using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    void IInteractable.DisplayInteractionPrompt()
    {
        Debug.Log("DisplayInteractionPrompt is not implemented yet");
    }

    void IInteractable.HideInteractionPrompt()
    {
        Debug.Log("HideInteractionPrompt is not implemented yet");
    }

    void IInteractable.Interact()
    {
        Debug.Log("Interact is not implemented yet");
    }
}
