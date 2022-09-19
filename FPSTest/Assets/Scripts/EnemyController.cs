using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int CurrentEnemyHealth = 5;
    private int _damageAmount;

    void Start()
    {
        _damageAmount = FindObjectOfType<GunController>().WeaponDamageOutput;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        // Enemy recieves damage if it is hit by bullet
        if (other.gameObject.tag == "Bullet")
        {
            EnemyTakeDamage(_damageAmount);

            if(CurrentEnemyHealth <= 0)
            {
                Disable();
            }
        }
    }

    void EnemyTakeDamage(int damageAmount)
    {
        CurrentEnemyHealth -= damageAmount;
    }
    void Disable()
    {
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }

}
