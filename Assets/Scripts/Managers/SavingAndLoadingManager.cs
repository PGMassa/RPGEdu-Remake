using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * THIS SCRIPT AND TEMPORARY IMPLEMENTATIONS THAT ONLY SERVES AS A MIDDLE-MAN 
 * BETWEEN THE "CONTINUE GAME" BUTTON ON THE MAIN MENU AND THE SCENE TRANSITION MANAGER
 */
public class SavingAndLoadingManager : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(SubscribeCallbacks());
    }

    private IEnumerator SubscribeCallbacks()
    {
        yield return new WaitUntil(() => EventManager.Instance != null);

        EventManager.Instance.savingAndLoadingEvents.OnLoadGameRequested += LoadGame;

        // Notify EventManager that UIManager is listening
        EventManager.Instance.internalEvents.ManagerStartedListening(gameObject.name);
    }

    // Later, this should receive the savefile and parse it, for now it only requests an transition to gomles
    private void LoadGame()
    {
        EventManager.Instance.sceneEvents.SceneTransitionRequested("Gomles");
    }
}
