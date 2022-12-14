using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    ObjectPoolManager ObjectPooler;
    public Transform FirePosition;
    public Transform MainCameraHead;
    private UICanvasController _theUICanvas;
    public Animator PlayerAnimator;

    public bool ActivateFullAuto;
    private bool _playerIsfiring, _playerCanFire = true;
    public float PauseBetweenShots = 0.2f;

    public int RoundsInMagazine, TotalRounds, MagazineCapacity, MaxAmmo;
    public float ReloadTime;
    public bool PlayerIsReloading;

    public string WeaponName;
    public string ProjectileTag;
    string WeaponAnimationName;

    public bool RocketLauncher;

    private int _groundAmmoPickupAmount;
    public string WeaponSoundEffectName;

    public bool UpgradedWeapon;

    // Start is called before the first frame update
    void Start()
    {
        // Using object pool singleton
        ObjectPooler = ObjectPoolManager.Instance;
        TotalRounds -= MagazineCapacity;
        RoundsInMagazine = MagazineCapacity;

        _theUICanvas = FindObjectOfType<UICanvasController>();

    }
    private void OnEnable()
    {
        // Stops bug where guns won't fire if swapped while reloading
        PlayerIsReloading = false;
        PlayerAnimator.SetBool("PlayerIsReloading", false);
        _playerCanFire = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }
        FireWeapon();
        WeaponManager();
        UpdateAmmoInfoText();
        WeaponAnimationManager();
    }

    private void WeaponAnimationManager()
    {
        switch (WeaponName)
        {
            case "Pistol":
                WeaponAnimationName = "ReloadPistol";
                break;

            case "Assault Rifle":
                WeaponAnimationName = "ReloadAssaultRifle";
                break;

            case "Upgraded Assault Rifle":
                WeaponAnimationName = "ReloadUpgradedAssaultRifle";
                break;

            case "Sniper Rifle":
                WeaponAnimationName = "ReloadSniperRifle";
                break;

            case "Rocket Launcher":
                WeaponAnimationName = "ReloadRocketLauncher";
                break;
        }

    }
    private void WeaponManager()
    {
        // Player can press R and reload at any time 
        if (Input.GetKeyDown(KeyCode.R) && RoundsInMagazine < MagazineCapacity && !PlayerIsReloading && TotalRounds != 0)
        {
            ReloadWeapon();
        }

        if (RoundsInMagazine == 0 && !PlayerIsReloading && TotalRounds != 0)
        {
            // Player will automatically reload when magazine is empty
            ReloadWeapon();
        }
    }
    #region Method: Fire Weapon
    private void FireWeapon()
    {
        if (ActivateFullAuto)
        {
            _playerIsfiring = Input.GetMouseButton(0);
        }
        else
        {
            _playerIsfiring = Input.GetMouseButtonDown(0);
        }

        if (_playerIsfiring && _playerCanFire && RoundsInMagazine > 0 && !PlayerIsReloading)
        {
            _playerCanFire = false;

            // Object pool will spawn different projectiles based on the tag associated with each weapon in the inspector
            if (RocketLauncher)
            {
                ObjectPooler.SpawnFromObjectPool(ProjectileTag, FirePosition.position, FirePosition.rotation);
                ObjectPooler.SpawnFromObjectPool("Rocket Trail Effect", FirePosition.position, FirePosition.rotation);
                AudioManager.Instance.PlaySoundOneShot($"{WeaponSoundEffectName}");
            }
            else
            {
                ObjectPooler.SpawnFromObjectPool(ProjectileTag, FirePosition.position, FirePosition.rotation);
                AudioManager.Instance.PlaySoundOneShot($"{WeaponSoundEffectName}");
            }
            // Raycast is determining what the bullet just hit, the origin and direction
            // are based of where the player is looking
            RaycastHit hit;

            if (Physics.Raycast(MainCameraHead.position, MainCameraHead.forward, out hit, 100f))
            {
                // Managing bullet accuracy based off distance and point of aim
                float distance = Vector3.Distance(MainCameraHead.position, hit.point);
                if (distance > 2f)
                {
                    FirePosition.LookAt(hit.point);

                    if (!RocketLauncher)
                    {
                        if (hit.collider.tag == "Shootable Object")
                        {
                            ObjectPooler.SpawnFromObjectPool("Bullet Hole", hit.point + (hit.normal * 0.025f), Quaternion.LookRotation(hit.normal));
                            //AudioManager.Instance.PlaySound("ShotImpactMetal");
                        }
                        else if (hit.collider.tag == "Floor")
                        {
                            ObjectPooler.SpawnFromObjectPool("Bullet Impact Ground", hit.point + (hit.normal * 0.025f), Quaternion.LookRotation(hit.normal));
                            //AudioManager.Instance.PlaySound("ShotImpactGround");
                        }
                    }
                }

                if (hit.collider.tag == "Enemy" && !RocketLauncher)
                {
                    ObjectPooler.SpawnFromObjectPool("Bullet Impact Flesh", hit.point, Quaternion.LookRotation(hit.normal));
                    AudioManager.Instance.PlaySoundOneShot("ShotImpactFlesh");
                }
            }
            else
            {
                // If the bullet hits nothing it still has a direction to fire
                FirePosition.LookAt(MainCameraHead.position + (MainCameraHead.forward * 50f));
            }

            RoundsInMagazine--;

            if (!RocketLauncher && !UpgradedWeapon)
            {
                ObjectPooler.SpawnFromObjectPool("Muzzle Flash", FirePosition.position, FirePosition.rotation).transform.parent = FirePosition;
            }

            if (UpgradedWeapon && !RocketLauncher)
            {
                ObjectPooler.SpawnFromObjectPool("Upgraded Muzzle Flash", FirePosition.position, FirePosition.rotation).transform.parent = FirePosition;
            }

            StartCoroutine(ResetShot());
        }

        if (_playerCanFire && _playerIsfiring && RoundsInMagazine == 0 && !PlayerIsReloading)
        {
            AudioManager.Instance.PlaySound("WeaponDryFire");
        }


    }
    #endregion
    public void AddAmmo()
    {
        _groundAmmoPickupAmount = MaxAmmo;

        // Set amount of ammo for each gun to pickup in inspector (on the weapon itself)
        TotalRounds += _groundAmmoPickupAmount;

        if (TotalRounds > MaxAmmo)
        {
            TotalRounds = MaxAmmo;
        }

        Debug.Log($"You recieve {_groundAmmoPickupAmount} rounds for the {WeaponName}");
    }
    private void ReloadWeapon()
    {
        PlayerAnimator.SetTrigger(WeaponAnimationName);

        PlayerIsReloading = true;

        PlayerAnimator.SetBool("PlayerIsReloading", true);

        StartCoroutine(ReloadCoroutine());
    }
    IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(PauseBetweenShots);
        _playerCanFire = true;
    }
    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(ReloadTime);

        int roundsToAddToMagazine = MagazineCapacity - RoundsInMagazine;

        if (TotalRounds > roundsToAddToMagazine)
        {
            TotalRounds -= roundsToAddToMagazine;
            RoundsInMagazine = MagazineCapacity;
        }
        else
        {
            RoundsInMagazine += TotalRounds;
            TotalRounds = 0;
        }
        PlayerAnimator.SetBool("PlayerIsReloading", false);
        PlayerIsReloading = false;
    }
    private void UpdateAmmoInfoText()
    {
        _theUICanvas.AmmoInfoText.SetText(RoundsInMagazine + " / " + MagazineCapacity);
        _theUICanvas.PlayersTotalAmmoText.SetText(TotalRounds.ToString());
    }
}
