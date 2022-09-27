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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Explosion Effect")
        {
            _damageAmount = other.gameObject.GetComponent<ExplosiveDamage>().ExplosionDamage;
        }
        else
        {
            _damageAmount = other.gameObject.GetComponent<BulletController>().ProjectileDamageOutput;
        }

        // Refactor this later

        switch (other.gameObject.tag)
        {
            case "Pistol Bullet":
                EnemyTakeDamage(_damageAmount);

                if (_currentEnemyHealth <= 0)
                {
                    EnemySpawnController.Instance.DecreaseActiveEnemyCount();
                    Destroy(gameObject);
                    //Disable();
                }
                break;

            case "Assault Rifle Bullet":
                EnemyTakeDamage(_damageAmount);

                if (_currentEnemyHealth <= 0)
                {
                    EnemySpawnController.Instance.DecreaseActiveEnemyCount();
                    Destroy(gameObject);
                    //Disable();
                }
                break;

            case "Sniper Rifle Bullet":
                EnemyTakeDamage(_damageAmount);

                if (_currentEnemyHealth <= 0)
                {
                    EnemySpawnController.Instance.DecreaseActiveEnemyCount();
                    Destroy(gameObject);
                    //Disable();
                }
                break;

            case "Explosion Effect":
                EnemyTakeDamage(_damageAmount);
                if (_currentEnemyHealth <= 0)
                {
                    EnemySpawnController.Instance.DecreaseActiveEnemyCount();
                    Destroy(gameObject);
                    //Disable();
                }
                break;

        }
    }

    public void EnemyTakeDamage(int damageAmount)
    {
        _currentEnemyHealth -= damageAmount;
        EnemyHealthBar.SetEnemyHealthBar(_currentEnemyHealth);
    }
    //void Disable()
    //{
    //    this.gameObject.SetActive(false);
    //}
    //private void OnDisable()
    //{
    //    CancelInvoke();
    //}

}
