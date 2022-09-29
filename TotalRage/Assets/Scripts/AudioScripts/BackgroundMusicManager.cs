using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour
{
    static BackgroundMusicManager Instance;
    public AudioClip[] backgroundMusicClips;

    public AudioSource AudioSource;
    public AudioSource ReplacementSource;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one background music manager in scene.");
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);

        ReplacementSource = gameObject.AddComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        switch (scene.name)
        {
            case "MainMenuScene":
                AudioSource.volume = 0;
                ReplacementSource.volume = 0;
                ReplacementSource.clip = backgroundMusicClips[1];
                StartCoroutine(FadeIn());
                break;

            default:
                AudioSource.volume = 0;
                ReplacementSource.volume = 0;
                ReplacementSource.clip = backgroundMusicClips[2];
                StartCoroutine(FadeIn());
                break;
        }

        if (ReplacementSource.clip != AudioSource.clip)
        {
            AudioSource.enabled = false;
            AudioSource.clip = ReplacementSource.clip;
            AudioSource.enabled = true;
        }
    }
    private IEnumerator FadeIn()
    {
        float speed = 0.02f;
        while (ReplacementSource.volume < 1 || AudioSource.volume < 1)
        {
            ReplacementSource.volume += speed;
            AudioSource.volume += speed;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
