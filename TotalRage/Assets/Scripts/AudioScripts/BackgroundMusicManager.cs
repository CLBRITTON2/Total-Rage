using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour
{
    static BackgroundMusicManager instance;
    public AudioClip[] backgroundMusicClips;

    public AudioSource audioSource;
    public AudioSource replacementSource;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one background music manager in scene.");
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        replacementSource = gameObject.AddComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        switch (scene.name)
        {
            case "PlayGround":
                replacementSource.clip = backgroundMusicClips[0];
                break;

            default:
                replacementSource.clip = backgroundMusicClips[0];
                break;
        }

        if (replacementSource.clip != audioSource.clip)
        {
            audioSource.enabled = false;
            audioSource.clip = replacementSource.clip;
            audioSource.enabled = true;
        }
    }
}
