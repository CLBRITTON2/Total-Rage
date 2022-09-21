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
        _damageAmount = other.gameObject.GetComponent<BulletController>().ProjectileDamageOutput;

        // Refactor this later

        switch (other.gameObject.tag)
        {
            case "Pistol Bullet":
                EnemyTakeDamage(_damageAmount);

                if (_currentEnemyHealth <= 0)
                {
                    Disable();
                }
                break;

            case "Assault Rifle Bullet":
                EnemyTakeDamage(_damageAmount);

                if (_currentEnemyHealth <= 0)
                {
                    Disable();
                }
                break;

            case "Sniper Rifle Bullet":
                EnemyTakeDamage(_damageAmount);

                if (_currentEnemyHealth <= 0)
                {
                    Disable();
                }
                break;
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
