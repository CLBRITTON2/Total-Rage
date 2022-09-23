using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;
    private AudioSource _audioSource;

    [Range(0, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolumeRange = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitchRange = 0.1f;
    public void SetAudioSource(AudioSource source)
    {
        _audioSource = source;
        _audioSource.clip = audioClip;
    }
    public void PlayAudio()
    {
        if (_audioSource == null)
        {
            return;
        }
        _audioSource.volume = volume * (1 + Random.Range(-randomVolumeRange / 2f, randomVolumeRange / 2f));
        _audioSource.pitch = pitch * (1 + Random.Range(-randomPitchRange / 2f, randomPitchRange / 2f)); ;
        _audioSource.Play();
    }
    public void StopAudio()
    {
        _audioSource.Stop();
    }
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] Sound[] sounds;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one audio manager in scene.");
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject gameObject = new GameObject($"Sound_{i}_{sounds[i].name}");
            gameObject.transform.SetParent(this.transform);
            // Avoid garbage collector
            sounds[i].SetAudioSource(gameObject.AddComponent<AudioSource>());
        }
    }
    private void Start()
    {

    }
    public void PlaySound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                sounds[i].PlayAudio();
                return;
            }
        }

        Debug.Log($"AudioManager: Sound not found in sound array: {name}");
    }
    public void StopSound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                sounds[i].StopAudio();
                return;
            }
        }
    }
}
