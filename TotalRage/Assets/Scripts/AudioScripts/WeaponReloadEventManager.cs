using UnityEngine;

public class WeaponReloadEventManager : MonoBehaviour
{
    private void PlayReloadCocking()
    {
        AudioManager.Instance.PlaySoundOneShot("ReloadCocking");
    }
    private void PlayReloadMagIn()
    {
        AudioManager.Instance.PlaySoundOneShot("ReloadMagIn");
    }
    private void PlayReloadMagOut()
    {
        AudioManager.Instance.PlaySoundOneShot("ReloadMagOut");
    }
    private void PlayReloadDecompress()
    {
        AudioManager.Instance.PlaySoundOneShot("ReloadDecompress");
    }
    private void PlayRocketLauncherMagIn()
    {
        AudioManager.Instance.PlaySoundOneShot("RocketLauncherMagIn");
    }
    private void PlayRocketLauncherMagOut()
    {
        AudioManager.Instance.PlaySoundOneShot("RocketLauncherMagOut");
    }
}
