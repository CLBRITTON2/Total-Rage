using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip AudioClip;
    private AudioSource _audioSource;
    public AudioRolloffMode AudioRollOff;

    [Range(0, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;
    [Range(0f, 1f)]
    public float SpatialBlend;

    [Range(0f, 0.5f)]
    public float randomVolumeRange = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitchRange = 0.1f;
    public void SetAudioSource(AudioSource source)
    {
        _audioSource = source;
        _audioSource.clip = AudioClip;
    }
    public void PlayAudioOneShot()
    {
        if (_audioSource == null)
        {
            return;
        }
        _audioSource.volume = volume * (1 + Random.Range(-randomVolumeRange / 2f, randomVolumeRange / 2f));
        _audioSource.pitch = pitch * (1 + Random.Range(-randomPitchRange / 2f, randomPitchRange / 2f));
        _audioSource.spatialBlend = SpatialBlend;;
        _audioSource.PlayOneShot(_audioSource.clip);
    }
    public void PlayAudio()
    {
        if (_audioSource == null)
        {
            return;
        }
        _audioSource.volume = volume * (1 + Random.Range(-randomVolumeRange / 2f, randomVolumeRange / 2f));
        _audioSource.pitch = pitch * (1 + Random.Range(-randomPitchRange / 2f, randomPitchRange / 2f));
        _audioSource.spatialBlend = SpatialBlend; ;
        _audioSource.Play();
    }
    public void StopAudio()
    {
        _audioSource.Stop();
    }
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] Sound[] Sounds;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one audio manager in scene.");
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < Sounds.Length; i++)
        {
            GameObject gameObject = new GameObject($"Sound_{i}_{Sounds[i].Name}");
            gameObject.transform.SetParent(this.transform);
            // Avoid garbage collector
            Sounds[i].SetAudioSource(gameObject.AddComponent<AudioSource>());
        }
    }
    private void Start()
    {

    }
    public void PlaySoundOneShot(string name)
    {
        for (int i = 0; i < Sounds.Length; i++)
        {
            if (Sounds[i].Name == name)
            {
                Sounds[i].PlayAudio();
                return;
            }
        }

        Debug.Log($"AudioManager: Sound not found in sound array: {name}");
    }
    public void PlaySound(string name)
    {
        for (int i = 0; i < Sounds.Length; i++)
        {
            if (Sounds[i].Name == name)
            {
                Sounds[i].PlayAudio();
                return;
            }
        }

        Debug.Log($"AudioManager: Sound not found in sound array: {name}");
    }
    public void StopSound(string name)
    {
        for (int i = 0; i < Sounds.Length; i++)
        {
            if (Sounds[i].Name == name)
            {
                Sounds[i].StopAudio();
                return;
            }
        }
    }
}
