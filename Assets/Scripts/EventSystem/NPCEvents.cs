using System;

public class NPCEvents
{
    // Event declarations
    public event Action<string> OnNPCDialogueRequested;

    // Raising events
    public void RequestNPCDialogue(string npcID) => OnNPCDialogueRequested?.Invoke(npcID);

}
