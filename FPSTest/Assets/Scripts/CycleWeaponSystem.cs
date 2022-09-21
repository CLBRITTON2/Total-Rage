using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CycleWeaponSystem : MonoBehaviour
{
    private WeaponController _activeWeapon;
    public List<WeaponController> AllWeapons = new List<WeaponController>();
    public int CurrentWeaponIndex;

    // Start is called before the first frame update
    void OnEnable()
    {
        // Deactivate all weapons on start then only activatre current weapon index
        foreach (WeaponController weapon in AllWeapons)
        {
            weapon.gameObject.SetActive(false);
        }
        _activeWeapon = AllWeapons[CurrentWeaponIndex];
        _activeWeapon.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchWeapon();
        }
    }
    private void SwitchWeapon()
    {
        _activeWeapon.gameObject.SetActive(false);
        CurrentWeaponIndex++;

        if (CurrentWeaponIndex >= AllWeapons.Count)
        {
            CurrentWeaponIndex = 0;
        }

        _activeWeapon = AllWeapons[CurrentWeaponIndex];
        _activeWeapon.gameObject.SetActive(true);
    }
}
