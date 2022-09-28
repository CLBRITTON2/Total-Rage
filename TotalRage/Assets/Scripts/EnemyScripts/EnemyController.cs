using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int EnemyMaxHealth;
    private int _currentEnemyHealth;
    private int _damageAmount;
    private int _enemyPointValue = 5;

    void Start()
    {
        _currentEnemyHealth = EnemyMaxHealth;
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
                    GameManager.PlayerPoints += _enemyPointValue;
                    EnemySpawnController.Instance.DecreaseActiveEnemyCount();
                    Destroy(gameObject);
                }
                break;

            case "Assault Rifle Bullet":
                EnemyTakeDamage(_damageAmount);

                if (_currentEnemyHealth <= 0)
                {
                    GameManager.PlayerPoints += _enemyPointValue;
                    EnemySpawnController.Instance.DecreaseActiveEnemyCount();
                    Destroy(gameObject);
                }
                break;

            case "Sniper Rifle Bullet":
                EnemyTakeDamage(_damageAmount);

                if (_currentEnemyHealth <= 0)
                {
                    GameManager.PlayerPoints += _enemyPointValue;
                    EnemySpawnController.Instance.DecreaseActiveEnemyCount();
                    Destroy(gameObject);
                }
                break;

            case "Explosion Effect":
                EnemyTakeDamage(_damageAmount);
                if (_currentEnemyHealth <= 0)
                {
                    GameManager.PlayerPoints += _enemyPointValue;
                    EnemySpawnController.Instance.DecreaseActiveEnemyCount();
                    Destroy(gameObject);
                }
                break;

        }
    }

    public void EnemyTakeDamage(int damageAmount)
    {
        _currentEnemyHealth -= damageAmount;
    }
}
