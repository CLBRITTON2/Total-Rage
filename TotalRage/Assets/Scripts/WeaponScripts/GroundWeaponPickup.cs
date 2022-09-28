using UnityEngine;

public class GroundWeaponPickup : MonoBehaviour
{
    public string GroundWeaponName;
    public int PointRequirement;
    public bool PlayerHasPointRequirement = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerHasPointRequirement)
        {
            other.gameObject.GetComponentInChildren<CycleWeaponSystem>().AddWeapon(GroundWeaponName);
            GameManager.PlayerPoints -= PointRequirement;
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (GameManager.PlayerPoints >= PointRequirement)
        {
            PlayerHasPointRequirement = true;
        }
    }
}
