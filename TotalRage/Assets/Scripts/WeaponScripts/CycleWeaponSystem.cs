using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CycleWeaponSystem : MonoBehaviour
{
    private WeaponController _activeWeapon;
    public List<WeaponController> AllWeapons = new List<WeaponController>();
    public int CurrentWeaponIndex;

    public List<WeaponController> GroundWeapons = new List<WeaponController>();

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
            AudioManager.Instance.PlaySound("EquipWeapon");
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
    public void AddWeapon(string weaponName)
    {
        bool unlocked = false;

        if (GroundWeapons.Count > 0)
        { 
            for(int i = 0; i < GroundWeapons.Count; i++)
            {
                if (GroundWeapons[i].WeaponName == weaponName)
                {
                    // Add ground weapon to all weapons list, remove it from ground weapons
                    // Set weapon to unlocked so player automatically equips ground weapons
                    AllWeapons.Add(GroundWeapons[i]);
                    GroundWeapons.RemoveAt(i);
                    i = GroundWeapons.Count;
                    unlocked = true;
                    Debug.Log($"Picked up: {weaponName}");
                }
            }
        }

        if(unlocked)
        {
            // Sets weapon index to always be the last weapon in the list
            // Player will always equip picked up ground weapons
            CurrentWeaponIndex = AllWeapons.Count - 2;
            SwitchWeapon();
        }
    }
}
