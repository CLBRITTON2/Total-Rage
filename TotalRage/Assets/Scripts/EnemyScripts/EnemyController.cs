using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [System.Serializable]
    public class EnemyLootTable
    {
        public string Name;
        public GameObject GroundConsumable;
        public int DropFrequency;
    }

    public int EnemyMaxHealth;
    private int _currentEnemyHealth;
    private int _damageAmount;
    private int _enemyPointValue = 5;
    public List<EnemyLootTable> GroundConsumables = new List<EnemyLootTable>();

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
                    KillEnemy();
                }
                break;

            case "Assault Rifle Bullet":
                EnemyTakeDamage(_damageAmount);

                if (_currentEnemyHealth <= 0)
                {
                    KillEnemy();
                }
                break;

            case "Sniper Rifle Bullet":
                EnemyTakeDamage(_damageAmount);

                if (_currentEnemyHealth <= 0)
                {
                    KillEnemy();
                }
                break;

            case "Explosion Effect":
                EnemyTakeDamage(_damageAmount);
                if (_currentEnemyHealth <= 0)
                {
                    KillEnemy();
                }
                break;

        }
    }
    private void KillEnemy()
    {
        GameManager.PlayerPoints += _enemyPointValue;
        EnemySpawnController.Instance.DecreaseActiveEnemyCount();
        CalculateConsumableDrop();
        Destroy(gameObject);
    }

    public void EnemyTakeDamage(int damageAmount)
    {
        _currentEnemyHealth -= damageAmount;
    }
    public void CalculateConsumableDrop()
    {
        // Create new position for dropped consumables so they animate at the correct height
        float xPos = transform.position.x;
        float yPos = transform.position.y + 1.0f;
        float zPos = transform.position.z;
        Vector3 consumableSpawnPoint = new Vector3(xPos, yPos, zPos);

        for (int i = 0; i < GroundConsumables.Count; i++)
        {
            int randomDropChance = Random.Range(0, 101);
            int randomItemDrop = Random.Range(0, GroundConsumables.Count);

            // Do not spawn a consumable
            if (randomDropChance > GroundConsumables[randomItemDrop].DropFrequency)
            {
                return;
            }
            // Spawn a consumable
            if (randomDropChance <= GroundConsumables[randomItemDrop].DropFrequency)
            {
                Debug.Log($"Dropped item {GroundConsumables[randomItemDrop].Name}");
                Instantiate(GroundConsumables[randomItemDrop].GroundConsumable, consumableSpawnPoint, Quaternion.identity, null);
                return;
            }
        }
    }
}
