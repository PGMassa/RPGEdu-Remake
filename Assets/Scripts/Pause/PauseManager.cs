using System.Collections;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;

    private void OnEnable()
    {
        // Doing it on a coroutine to avoid "execution order" shenanigans
        StartCoroutine(SubscribeCallbacks());
    }

    private IEnumerator SubscribeCallbacks()
    {
        yield return new WaitUntil(() => EventManager.Instance != null);

        EventManager.Instance.uiEvents.OnActiveCanvasChanged += TogglePause;
    }

    private void TogglePause(string canvasName)
    {
        Time.timeScale = pauseCanvas.name.Equals(canvasName) ? 0 : 1;
    }
}
