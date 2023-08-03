using System;

public class SceneEvents
{
    // Event declarations
    public event Action<string> OnSceneTransitionRequested;
    public event Action OnUnloadGameSceneRequested;

    public event Action<string> OnGameSceneLoaded;
    public event Action<string> OnGameSceneUnloaded;

    // Raising events
    public void SceneTransitionRequested(string newScene) => OnSceneTransitionRequested?.Invoke(newScene);
    public void UnloadGameSceneRequested() => OnUnloadGameSceneRequested?.Invoke();

    public void GameSceneLoaded(string newScene) => OnGameSceneLoaded?.Invoke(newScene);
    public void GameSceneUnloaded(string oldScene) => OnGameSceneUnloaded?.Invoke(oldScene);
}
