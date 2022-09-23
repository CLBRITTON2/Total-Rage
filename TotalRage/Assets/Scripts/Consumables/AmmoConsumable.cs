using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoConsumable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySound("PickUpAmmoConsumable");
            other.GetComponentInChildren<WeaponController>().AddAmmo();
            Destroy(gameObject);
        }
    }
}
