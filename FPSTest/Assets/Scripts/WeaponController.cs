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

    public bool ActivateFullAuto;
    private bool _playerIsfiring, _playerCanFire = true;
    public float PauseBetweenShots = 0.2f;

    public int RoundsInMagazine, TotalRounds, MagazineCapacity;
    public float ReloadTime;
    public bool PlayerIsReloading;

    public Transform AimPoint;
    private float _aimDownSightSpeed = 3f;
    private Vector3 _weaponStartPosition;
    public float ZoomMagnification;

    public string WeaponName;

    // Start is called before the first frame update
    void Start()
    {
        ObjectPooler = ObjectPoolManager.Instance;
        TotalRounds -= MagazineCapacity;
        RoundsInMagazine = MagazineCapacity;

        _weaponStartPosition = transform.localPosition;

        _theUICanvas = FindObjectOfType<UICanvasController>();
    }
    private void OnEnable()
    {
        // Stops bug where guns won't fire if swapped while reloading
        PlayerIsReloading = false;
        _playerCanFire = true;
    }
    // Update is called once per frame
    void Update()
    {
        FireWeapon();
        WeaponManager();
        UpdateAmmoInfoText();
    }
    private void WeaponManager()
    {
        if(Input.GetKeyDown(KeyCode.R) && RoundsInMagazine < MagazineCapacity && !PlayerIsReloading)
        {
            ReloadWeapon();
        }

        if(Input.GetMouseButton(1))
        {
            transform.position = Vector3.MoveTowards(transform.position, AimPoint.position, _aimDownSightSpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _weaponStartPosition, _aimDownSightSpeed * Time.deltaTime);
        }

        if(Input.GetMouseButtonDown(1))
        {
            FindObjectOfType<CameraMove>().ZoomIn(ZoomMagnification);
        }
        if(Input.GetMouseButtonUp(1))
        {
            FindObjectOfType<CameraMove>().ZoomOut();
        }
    }
    private void FixedUpdate()
    {

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

            // Switch statement for different guns to pull different rounds from the object pool
            switch (WeaponName)
            {
                case "Pistol":
                    ObjectPooler.SpawnFromObjectPool("Pistol Bullet", FirePosition.position, FirePosition.rotation).transform.parent = FirePosition;
                    break;

                case "AssaultRifle":
                    ObjectPooler.SpawnFromObjectPool("Assault Rifle Bullet", FirePosition.position, FirePosition.rotation).transform.parent = FirePosition;
                    break;

                case "SniperRifle":
                    ObjectPooler.SpawnFromObjectPool("Sniper Rifle Bullet", FirePosition.position, FirePosition.rotation).transform.parent = FirePosition;
                    break;

                case "RocketLauncher":
                    ObjectPooler.SpawnFromObjectPool("Rocket Launcher Bullet", FirePosition.position, FirePosition.rotation).transform.parent = FirePosition;
                    break;
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

                    if (hit.collider.tag == "Shootable Object")
                    {
                        ObjectPooler.SpawnFromObjectPool("Bullet Hole", hit.point + (hit.normal * 0.025f), Quaternion.LookRotation(hit.normal));
                    }
                    else if (hit.collider.tag == "Floor")
                    {
                        ObjectPooler.SpawnFromObjectPool("Bullet Impact Ground", hit.point + (hit.normal * 0.025f), Quaternion.LookRotation(hit.normal));
                    }
                    else if (hit.collider.tag == "Enemy")
                    {
                        ObjectPooler.SpawnFromObjectPool("Bullet Impact Flesh", hit.point, Quaternion.LookRotation(hit.normal));
                    }
                }
            }
            else
            {
                // If the bullet hits nothing it still has a direction to fire
                FirePosition.LookAt(MainCameraHead.position + (MainCameraHead.forward * 50f));
            }
            RoundsInMagazine--;

            Instantiate(MuzzleFlash, FirePosition.position, FirePosition.rotation, FirePosition);

            StartCoroutine(ResetShot());
        }
    }
    #endregion
    private void ReloadWeapon()
    {
        int roundsToAddToMagazine = MagazineCapacity - RoundsInMagazine;

        if(TotalRounds > roundsToAddToMagazine)
        {
            TotalRounds -= roundsToAddToMagazine;
            RoundsInMagazine = MagazineCapacity;
        }
        else
        {
            RoundsInMagazine += TotalRounds;
            TotalRounds = 0;
        }

        PlayerIsReloading = true;
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
        PlayerIsReloading = false;
    }
    private void UpdateAmmoInfoText()
    {
        _theUICanvas.AmmoInfoText.SetText(RoundsInMagazine + " / " + MagazineCapacity);
        _theUICanvas.PlayersTotalAmmoText.SetText(TotalRounds.ToString());
    }
}
