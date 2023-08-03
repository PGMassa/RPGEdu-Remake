using UnityEngine;

public class MainMenuUI : ICanvasController
{
    private Canvas mainMenuCanvas;

    public GameObject canvasObject => mainMenuCanvas.gameObject;

    public MainMenuUI(Canvas mainMenuCanvas)
    {
        this.mainMenuCanvas = mainMenuCanvas;
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
    public void OnNewGame()
    {
        Debug.Log("New game not implemented yet");
    }

    public void OnContinueGame()
    {
        EventManager.Instance.savingAndLoadingEvents.LoadGameRequested();
    }

    public void OnOpenOptions()
    {
        Debug.Log("Options not implemented yet");
    }

    public void OnOpenCredits()
    {
        Debug.Log("Credits not implemented yet");
    }

    public void OnOpenAbout()
    {
        Debug.Log("About not implemented yet");
    }

    public void OnExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}
