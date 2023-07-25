using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private void OnEnable()
    {
        // Doing it on a coroutine to avoid "execution order" shenanigans
        StartCoroutine(SubscribeCallbacks());
    }

    private IEnumerator SubscribeCallbacks()
    {
        yield return new WaitUntil(() => EventManager.Instance != null);

        EventManager.Instance.uiEvents.OnPauseMenuOpened += () => TogglePause(true);
        EventManager.Instance.uiEvents.OnPauseMenuClosed += () => TogglePause(false);
    }

    private void TogglePause(bool pauseGame)
    {
        Time.timeScale = pauseGame ? 0 : 1;
    }
}
