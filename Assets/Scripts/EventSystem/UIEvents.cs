using System;
using UnityEngine;

public class UIEvents
{
    // Event declarations
    public event Action<string, string> OnDisplayInteractionPromptRequest;
    public event Action<string, string> OnHideInteractionPromptRequest;
    public event Action<Sprite> OnNPCInterfaceChangeRequest; // Change the dialogue box depending on the npc currently on dialogue

    public event Action OnPauseMenuOpened;
    public event Action OnPauseMenuClosed;

    // Raising events
    public void DisplayInteractionPromptRequest(string requesterID, string message) => OnDisplayInteractionPromptRequest?.Invoke(requesterID, message);
    public void HideInteractionPromptRequest(string requesterID, string message) => OnHideInteractionPromptRequest?.Invoke(requesterID, message);
    public void NPCInterfaceChangeRequest(Sprite dialogueBox) => OnNPCInterfaceChangeRequest?.Invoke(dialogueBox);

    public void PauseMenuOpened() => OnPauseMenuOpened?.Invoke();
    public void PauseMenuClosed() => OnPauseMenuClosed?.Invoke();
}
