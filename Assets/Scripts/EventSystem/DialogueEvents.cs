using System;
using System.Collections.Generic;

public class DialogueEvents
{
    // Event declarations
    public event Action OnDialogueStarted;
    public event Action OnDialogueEnded;
    public event Action<string> OnNextDialogueLine;
    public event Action<List<string>> OnDialogueChoicesEnabled; // one frame after the dialogue reaches an choice
    public event Action OnDialogueChoicesDisabled; // one frame after the player makes a choice

    // Raising events
    public void DialogueStarted() => OnDialogueStarted?.Invoke();
    public void DialogueEnded() => OnDialogueEnded?.Invoke();
    public void NextDialogueLine(string nextLine) => OnNextDialogueLine?.Invoke(nextLine);
    public void DialogueChoicesEnabled(List<string> choices) => OnDialogueChoicesEnabled(choices);
    public void DialogueChoicesDisabled() => OnDialogueChoicesDisabled?.Invoke();

}



