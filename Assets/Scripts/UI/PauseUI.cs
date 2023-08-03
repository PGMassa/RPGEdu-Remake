using UnityEngine;

public class PauseUI : ICanvasController
{
    private Canvas pauseCanvas;

    public GameObject canvasObject => pauseCanvas.gameObject;

    public PauseUI(Canvas pauseCanvas)
    {
        this.pauseCanvas = pauseCanvas;
    }

    void ICanvasController.StartCanvas()
    {
        canvasObject.SetActive(true);
    }

    void ICanvasController.CloseCanvas()
    {
        canvasObject.SetActive(false);
    }

    // Callbacks from the UI buttons
    public void OnReturnToGame()
    {
        EventManager.Instance.inputEvents.PausePerformed();
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

    /*public void OnOpenOptions()
    {
        Debug.Log("Options UI not implemented yet");
    }*/

    public void OnExitToMenu()
    {
        EventManager.Instance.sceneEvents.UnloadGameSceneRequested();
    }
}
