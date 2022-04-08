using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager singleton;
    // Start is called before the first frame update
    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void LoadGameSceneAsync(Action<AsyncOperation> action)
    {
        StartCoroutine(LoadGameSceneAsyncCoroutine(action));
    }

    IEnumerator LoadGameSceneAsyncCoroutine(Action<AsyncOperation> action)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("GameScene");

        op.allowSceneActivation = true;
        op.completed += action;
        while (op.isDone == false)
        {
            yield return null;
        }
        
    }
}
