using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    ObjectPoolManager objectPooler;
    public Transform firePosition;
    public Transform mainCameraHead;
    public GameObject muzzleFlash;

    public bool activateFullAuto;
    private bool playerIsfiring, playerCanFire = true;
    public float pauseBetweenShots = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPoolManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        FireWeapon();
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

        if (playerIsfiring && playerCanFire)
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

            Instantiate(muzzleFlash, firePosition.position, firePosition.rotation, firePosition);
            StartCoroutine(ResetShot());
        }
    }
    #endregion
    IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(pauseBetweenShots);
        playerCanFire = true;
    }
}
