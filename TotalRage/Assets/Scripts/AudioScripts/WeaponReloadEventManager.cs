using UnityEngine;

public class WeaponReloadEventManager : MonoBehaviour
{
    private void PlayReloadCocking()
    {
        AudioManager.Instance.PlaySound("ReloadCocking");
    }
    private void PlayReloadMagIn()
    {
        AudioManager.Instance.PlaySound("ReloadMagIn");
    }
    private void PlayReloadMagOut()
    {
        AudioManager.Instance.PlaySound("ReloadMagOut");
    }
    private void PlayReloadDecompress()
    {
        AudioManager.Instance.PlaySound("ReloadDecompress");
    }
    private void PlayRocketLauncherMagIn()
    {
        AudioManager.Instance.PlaySound("RocketLauncherMagIn");
    }
    private void PlayRocketLauncherMagOut()
    {
        AudioManager.Instance.PlaySound("RocketLauncherMagOut");
    }
}
