using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundWeaponPickup : MonoBehaviour
{
    public string GroundWeaponName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponentInChildren<CycleWeaponSystem>().AddWeapon(GroundWeaponName);
            Destroy(gameObject);
        }
    }
}
