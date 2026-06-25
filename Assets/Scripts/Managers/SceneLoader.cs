using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    [SerializeField] private string loadingSceneName = "LoadingScene";

    [Header("UI")]
    private Slider _progressBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string targetSceneName)
    {
        StartCoroutine(LoadSceneRoutine(targetSceneName));
    }

    private IEnumerator LoadSceneRoutine(string targetScene)
    {
        AsyncOperation loadingSceneOp = SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);
        while (!loadingSceneOp.isDone)
            yield return null;

        //Suche nach ProgressBar in der geladenen Szene
        var loadingScene = SceneManager.GetSceneByName(loadingSceneName);
        GameObject[] rootObjects = loadingScene.GetRootGameObjects();
        foreach (GameObject go in rootObjects)
        {
            Slider slider = go.GetComponentInChildren<Slider>();
            if (slider != null)
            {
                _progressBar = slider;
                break;
            }
        }

        if (_progressBar == null)
        {
            Log.Warning("ProgressBar not found in LoadingScene!");
        }

        //Zielszene asynchron laden (aber noch nicht aktivieren)
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
        loadOp.allowSceneActivation = false;

        while (loadOp.progress < 0.9f)
        {
            if (_progressBar != null)
            {
                _progressBar.value = Mathf.Clamp01(loadOp.progress / 0.9f);
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        loadOp.allowSceneActivation = true;
        while (!loadOp.isDone) yield return null;
        SceneManager.UnloadSceneAsync(loadingSceneName);
    }
}