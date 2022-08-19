using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    ObjectPoolManager objectPooler;
    public Transform firePosition;
    public Transform mainCameraHead;
    public GameObject muzzleFlash;
    private UICanvasController theUICanvas;

    public bool activateFullAuto;
    private bool playerIsfiring, playerCanFire = true;
    public float pauseBetweenShots = 0.2f;

    public int roundsInMagazine, totalRounds, magazineCapacity;
    public float reloadTime;
    public bool playerIsReloading;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPoolManager.instance;
        totalRounds -= magazineCapacity;
        roundsInMagazine = magazineCapacity;

        theUICanvas = FindObjectOfType<UICanvasController>();
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
        if(Input.GetKeyDown(KeyCode.R) && roundsInMagazine < magazineCapacity && !playerIsReloading)
        {
            ReloadWeapon();
        }
    }
    private void FixedUpdate()
    {

    }
    #region Method: Fire Weapon
    private void FireWeapon()
    {
        if(activateFullAuto)
        {
            playerIsfiring = Input.GetMouseButton(0);
        }
        else
        {
            playerIsfiring = Input.GetMouseButtonDown(0);
        }

        if (playerIsfiring && playerCanFire && roundsInMagazine > 0 && !playerIsReloading)
        {
            playerCanFire = false;

            objectPooler.SpawnFromObjectPool("Bullet", firePosition.position, firePosition.rotation).transform.parent = firePosition;
            // Raycast is determining what the bullet just hit, the origin and direction
            // are based of where the player is looking
            RaycastHit hit;

            if (Physics.Raycast(mainCameraHead.position, mainCameraHead.forward, out hit, 100f))
            {
                // Managing bullet accuracy based off distance and point of aim
                float distance = Vector3.Distance(mainCameraHead.position, hit.point);
                if (distance > 2f)
                {
                    firePosition.LookAt(hit.point);

                    if (hit.collider.tag == "Shootable Object")
                    {
                        objectPooler.SpawnFromObjectPool("Bullet Hole", hit.point + (hit.normal * 0.025f), Quaternion.LookRotation(hit.normal));
                    }
                    else if (hit.collider.tag == "Floor")
                    {
                        objectPooler.SpawnFromObjectPool("Bullet Impact Ground", hit.point + (hit.normal * 0.025f), Quaternion.LookRotation(hit.normal));
                    }
                }
            }
            else
            {
                // If the bullet hits nothing it still has a direction to fire
                firePosition.LookAt(mainCameraHead.position + (mainCameraHead.forward * 50f));
            }
            roundsInMagazine--;

            Instantiate(muzzleFlash, firePosition.position, firePosition.rotation, firePosition);

            StartCoroutine(ResetShot());
        }
    }
    #endregion
    private void ReloadWeapon()
    {
        int roundsToAddToMagazine = magazineCapacity - roundsInMagazine;

        if(totalRounds > roundsToAddToMagazine)
        {
            totalRounds -= roundsToAddToMagazine;
            roundsInMagazine = magazineCapacity;
        }
        else
        {
            roundsInMagazine += totalRounds;
            totalRounds = 0;
        }

        playerIsReloading = true;
        StartCoroutine(ReloadCoroutine());
    }
    IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(pauseBetweenShots);
        playerCanFire = true;
    }
    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);
        playerIsReloading = false;
    }
    private void UpdateAmmoInfoText()
    {
        theUICanvas.ammoInfoText.SetText(roundsInMagazine + " / " + magazineCapacity);
        theUICanvas.playersTotalAmmoText.SetText(totalRounds.ToString());
    }
}
