using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

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
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
        loadOp.allowSceneActivation = false;

        yield return new WaitForSeconds(0.3f);
        loadOp.allowSceneActivation = true;
        while (!loadOp.isDone) yield return null;
    }
}