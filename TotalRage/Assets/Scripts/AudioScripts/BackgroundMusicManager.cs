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
            case "PlayGround":
                ReplacementSource.clip = backgroundMusicClips[0];
                break;

            case "MainMenuScene":
                ReplacementSource.enabled = false;
                AudioSource.enabled = false;
                break;

            default:
                ReplacementSource.clip = backgroundMusicClips[0];
                break;
        }

        if (ReplacementSource.clip != AudioSource.clip)
        {
            AudioSource.enabled = false;
            AudioSource.clip = ReplacementSource.clip;
            AudioSource.enabled = true;
        }
    }
}
