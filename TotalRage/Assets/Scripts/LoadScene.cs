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
    private float _loadTime = 0f;
    public CanvasGroup LoadScreenCanvasGroup;

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
        _loadingBarFiller.fillAmount = 0f;
        StartCoroutine(LoadSceneRoutine(sceneName));
    }
    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        LoadingScreenCanvas.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(0, 3));
        _operation = SceneManager.LoadSceneAsync(sceneName);
        _operation.allowSceneActivation = false;

        while (!_operation.isDone)
        {
            _loadTime += Time.deltaTime;

            float progress = Mathf.Clamp01(_operation.progress / 0.9f);

            _loadingBarFiller.fillAmount = progress;

            if (_loadTime >= 5f)
            {
                yield return StartCoroutine(FadeLoadingScreen(1, 3));
                _operation.allowSceneActivation = true;
            }
            yield return null;
        }
        _operation = null;
        LoadingScreenCanvas.SetActive(false);

        if (sceneName != "MainMenuScene")
        {
            AudioManager.Instance.PlaySound("Laughter");
        }

        yield return StartCoroutine(FadeLoadingScreen(0, 3));
    }
    private IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = LoadScreenCanvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            LoadScreenCanvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        LoadScreenCanvasGroup.alpha = targetValue;
    }
}
