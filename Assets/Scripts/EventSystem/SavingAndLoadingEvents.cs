using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingAndLoadingEvents
{
    // Event declarations
    public event Action OnLoadGameRequested;

    // Raising events
    public void LoadGameRequested() => OnLoadGameRequested?.Invoke();
}
