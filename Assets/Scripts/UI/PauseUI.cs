using UnityEngine;

public class PauseUI
{
    private Canvas pauseCanvas;

    public PauseUI(Canvas pauseCanvas)
    {
        this.pauseCanvas = pauseCanvas;
    }

    public void TogglePauseCanvas()
    {
        pauseCanvas.gameObject.SetActive(!pauseCanvas.gameObject.activeSelf);

        if (pauseCanvas.gameObject.activeSelf) EventManager.Instance.uiEvents.PauseMenuOpened();
        else EventManager.Instance.uiEvents.PauseMenuClosed();
    }

    // Callbacks from the UI buttons
    public void OnReturnToGame()
    {
        Debug.Log("return to game not implemented yet");
    }

    public void OnSave()
    {
        Debug.Log("Save system not implemented yet");
    }

    public void OnOpenInventory()
    {
        Debug.Log("Inventary UI not implemented yet");
    }

    public void OnOpenQuests()
    {
        Debug.Log("Quest UI not implemented yet");
    }

    public void OnOpenStates()
    {
        Debug.Log("Estados not implemented yet. By the way, what is supposed to be in this interface? It's not implemented in the original project");
    }

    public void OnOpenMap()
    {
        Debug.Log("Map UI not implemented yet");
    }

    public void OnOpenOptions()
    {
        Debug.Log("Options UI not implemented yet");
    }

    public void OnExit()
    {
        Debug.Log("Main Menu not implemented yet");
    }
}
