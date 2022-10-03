using TMPro;
using UnityEngine;

public class GroundWeaponPickup : MonoBehaviour
{
    public string GroundWeaponName;
    public int PointRequirement;
    public bool PlayerHasPointRequirement = false;
    private TextMeshProUGUI _purchaseDisplayText;
    public GameObject PurchaseTextContainer;

    private void Start()
    {
        _purchaseDisplayText = FindObjectOfType<UICanvasController>().PurchaseText;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && PlayerHasPointRequirement)
        {
            PurchaseTextContainer.SetActive(true);
            _purchaseDisplayText.SetText($"Press E to buy a(n) {GroundWeaponName} for {PointRequirement} points");

            if (Input.GetKeyDown(KeyCode.E))
            {
                AudioManager.Instance.PlaySoundOneShot("PurchaseSound");
                other.gameObject.GetComponentInChildren<CycleWeaponSystem>().AddWeapon(GroundWeaponName);
                GameManager.PlayerPoints -= PointRequirement;
                PurchaseTextContainer.SetActive(false);
                Destroy(gameObject);
            }
        }

        if (other.CompareTag("Player") && !PlayerHasPointRequirement)
        {
            PurchaseTextContainer.SetActive(true);
            _purchaseDisplayText.SetText($"You need {PointRequirement} points to buy a(n) {GroundWeaponName}");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PurchaseTextContainer.SetActive(false);
    }
    private void Update()
    {
        if (GameManager.PlayerPoints >= PointRequirement)
        {
            PlayerHasPointRequirement = true;
        }
    }
}
