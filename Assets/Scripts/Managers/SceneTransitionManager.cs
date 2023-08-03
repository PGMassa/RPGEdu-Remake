using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransitionManager : MonoBehaviour
{
    // keeps track of the wich gameScene is loaded. There can be only one!
    private string currentLoadedGameScene;

    private void OnEnable()
    {
        StartCoroutine(SubscribeCallbacks());
    }

    private IEnumerator SubscribeCallbacks()
    {
        yield return new WaitUntil(() => EventManager.Instance != null);

        EventManager.Instance.sceneEvents.OnSceneTransitionRequested += newScene => StartCoroutine(ChangeGameScene(newScene));
        EventManager.Instance.sceneEvents.OnUnloadGameSceneRequested += () => StartCoroutine(UnloadCurrentActiveGameScene());

        EventManager.Instance.internalEvents.ManagerStartedListening(gameObject.name);
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

        EventManager.Instance.sceneEvents.GameSceneLoaded(scene);
    }

    // Unload current gameScene. The main gameScene (with the managers) is kept active -> used to return to main menu
    private IEnumerator UnloadCurrentActiveGameScene()
    {
        if(currentLoadedGameScene != null)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currentLoadedGameScene);

            yield return new WaitUntil(() => asyncUnload.isDone);

            EventManager.Instance.sceneEvents.GameSceneUnloaded(currentLoadedGameScene);
            currentLoadedGameScene = null;
        }


    }

}
