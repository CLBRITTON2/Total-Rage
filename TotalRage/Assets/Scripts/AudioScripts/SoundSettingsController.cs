using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundSettingsController : MonoBehaviour
{
    [SerializeField] private TMP_Text _volumeTextValue = null;
    [SerializeField] private Slider _volumeSlider = null;
    [SerializeField] private float _defaultVolume = 1f;

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        _volumeTextValue.text = volume.ToString("0.0");
    }
    public void ResetVolumeButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = _defaultVolume;
            _volumeSlider.value = _defaultVolume;
            _volumeTextValue.text = _defaultVolume.ToString("0.0");
            ApplyVolume();
        }
    }
    public void ApplyVolume()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }
}
