using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveDamage : MonoBehaviour
{
    public int ExplosionDamage;

    private void OnTriggerEnter(Collider other)
    {
        // Enemy damage is handled in enemy controller
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealthSystem>().PlayerTakeDamage(ExplosionDamage);
        }
    }
}
