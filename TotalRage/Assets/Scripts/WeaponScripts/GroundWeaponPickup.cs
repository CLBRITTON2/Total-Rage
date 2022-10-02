using UnityEngine;

public class GroundWeaponPickup : MonoBehaviour
{
    public string GroundWeaponName;
    public int PointRequirement;
    public bool PlayerHasPointRequirement = false;
    private UICanvasController _purchaseDisplayText;

    private void Start()
    {
        _purchaseDisplayText = FindObjectOfType<UICanvasController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        _purchaseDisplayText.PurchaseText.enabled = true;

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
