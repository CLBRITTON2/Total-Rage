using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    ObjectPoolManager ObjectPooler;
    public Transform FirePosition;
    public Transform MainCameraHead;
    public GameObject MuzzleFlash;
    private UICanvasController _theUICanvas;
    public Animator PlayerAnimator;

    public bool ActivateFullAuto;
    private bool _playerIsfiring, _playerCanFire = true;
    public float PauseBetweenShots = 0.2f;

    public int RoundsInMagazine, TotalRounds, MagazineCapacity;
    public float ReloadTime;
    public bool PlayerIsReloading;

    public string WeaponName;
    public string ProjectileTag;
    string WeaponAnimationName;

    public bool RocketLauncher;

    public int GroundAmmoPickupAmount;
    public string WeaponSoundEffectName;

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

            case "AssaultRifle":
                WeaponAnimationName = "ReloadAssaultRifle";
                break;

            case "SniperRifle":
                WeaponAnimationName = "ReloadSniperRifle";
                break;

            case "RocketLauncher":
                WeaponAnimationName = "ReloadRocketLauncher";
                break;
        }

    }
    private void WeaponManager()
    {
        // Player can press R and reload at any time 
        if(Input.GetKeyDown(KeyCode.R) && RoundsInMagazine < MagazineCapacity && !PlayerIsReloading)
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
        if(ActivateFullAuto)
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
                AudioManager.Instance.PlaySound($"{WeaponSoundEffectName}");
            }
            else
            {
                ObjectPooler.SpawnFromObjectPool(ProjectileTag, FirePosition.position, FirePosition.rotation).transform.parent = FirePosition;
                AudioManager.Instance.PlaySound($"{WeaponSoundEffectName}");
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
                            AudioManager.Instance.PlaySound("ShotImpactMetal");
                        }
                        else if (hit.collider.tag == "Floor")
                        {
                            ObjectPooler.SpawnFromObjectPool("Bullet Impact Ground", hit.point + (hit.normal * 0.025f), Quaternion.LookRotation(hit.normal));
                        }
                    }
                }

                if (hit.collider.tag == "Enemy" && !RocketLauncher)
                {
                    ObjectPooler.SpawnFromObjectPool("Bullet Impact Flesh", hit.point, Quaternion.LookRotation(hit.normal));
                    AudioManager.Instance.PlaySound("ShotImpactFlesh");
                }
            }
            else
            {
                // If the bullet hits nothing it still has a direction to fire
                FirePosition.LookAt(MainCameraHead.position + (MainCameraHead.forward * 50f));
            }

            RoundsInMagazine--;

            if (!RocketLauncher)
            {
                //Instantiate(MuzzleFlash, FirePosition.position, FirePosition.rotation, FirePosition);
                ObjectPooler.SpawnFromObjectPool("Muzzle Flash", FirePosition.position, FirePosition.rotation).transform.parent = FirePosition;
            }

            StartCoroutine(ResetShot());
        }
    }
    #endregion
    public void AddAmmo()
    {
        // Set amount of ammo for each gun to pickup in inspector (on the weapon itself)
        TotalRounds += GroundAmmoPickupAmount;
        Debug.Log($"You recieve {GroundAmmoPickupAmount} rounds for the {WeaponName}");
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
