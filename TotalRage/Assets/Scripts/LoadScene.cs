using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public static LoadScene Instance;
    public  GameObject LoadingScreenCanvas;
    [SerializeField] private Image _loadingBarFiller;
    private AsyncOperation _operation;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one load screen instance");
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
    public void LoadNextScene(string sceneName)
    {
        _loadingBarFiller.fillAmount = 0;
        LoadingScreenCanvas.SetActive(true);
        StartCoroutine(LoadSceneRoutine(sceneName));
    }
    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        _operation = SceneManager.LoadSceneAsync(sceneName);

        while(!_operation.isDone)
        {
            float progress = _operation.progress;

            _loadingBarFiller.fillAmount = progress;

            yield return null;
        }
        _operation = null;
        LoadingScreenCanvas.SetActive(false);
    }
}
