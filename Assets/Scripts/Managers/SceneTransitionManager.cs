using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransitionManager : MonoBehaviour
{
    [Header("First scene to be loaded")]
    [SerializeField] string firstScene;

    // keeps track of the wich gameScene is loaded. There can be only one!
    private string currentLoadedGameScene;

    private void OnEnable()
    {
        StartCoroutine(SubscribeCallbacks());
    }

    private IEnumerator SubscribeCallbacks()
    {
        yield return new WaitUntil(() => EventManager.Instance != null);

        // Subscribe to get notified when all the managers are ready
        EventManager.Instance.internalEvents.OnAllManagersReady += () => StartCoroutine(ChangeGameScene(firstScene));

        EventManager.Instance.internalEvents.ManagerStartedListening(gameObject.name);
    }

    private void OnDisable()
    {
        // Unsubscribe to get notified when all the managers are ready
        EventManager.Instance.internalEvents.OnAllManagersReady -= () => StartCoroutine(ChangeGameScene(firstScene));

        // Notify EventManager that SceneTransitionManager is listening
        EventManager.Instance.internalEvents.ManagerStoppedListening(gameObject.name);
    }

    /*
     * A GameScene contains everything the player can see, they are the different levels and locations the player can visit.
     * GameScenes are loaded additively to ManagerScenes, and therefore can only be loaded after every ManagerScene is already loaded.
     * Only one GameScene can be loaded at each time, therefore we must also unload the previous GameScene
     */
    private IEnumerator ChangeGameScene(string scene)
    {
        // Unloading previous scene (if there is one)
        if (currentLoadedGameScene != null)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currentLoadedGameScene);
            //yield return new WaitUntil(() => asyncUnload.isDone);
        }

        // Starting to load the next scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Wait until it's mostly loaded before allowing activation
        yield return new WaitUntil(() => asyncLoad.progress >= 0.9f);
        asyncLoad.allowSceneActivation = true;

        currentLoadedGameScene = scene;
    }

}
