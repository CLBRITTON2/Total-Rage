using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int EnemyMaxHealth;
    private int _currentEnemyHealth;
    private int _damageAmount;

    EnemyUICanvasController EnemyHealthBar;

    void Start()
    {
        _damageAmount = FindObjectOfType<GunController>().WeaponDamageOutput;
        EnemyHealthBar = GetComponentInChildren<EnemyUICanvasController>();

        _currentEnemyHealth = EnemyMaxHealth;

        EnemyHealthBar.SetEnemyMaxHealth(EnemyMaxHealth);
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

            if(_currentEnemyHealth <= 0)
            {
                Disable();
            }
        }
    }

    void EnemyTakeDamage(int damageAmount)
    {
        _currentEnemyHealth -= damageAmount;
        EnemyHealthBar.SetEnemyHealthBar(_currentEnemyHealth);
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
