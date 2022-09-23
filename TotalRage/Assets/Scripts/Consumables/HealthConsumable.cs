using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthConsumable : MonoBehaviour
{
    public int AmountToHeal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealthSystem>().HealPlayer(AmountToHeal);
            Debug.Log($"You've healed {AmountToHeal} hitpoints");
            Destroy(gameObject);
        }
    }
}
