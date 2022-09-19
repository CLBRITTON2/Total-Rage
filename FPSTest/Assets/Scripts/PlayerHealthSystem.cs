using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    public int PlayerMaxHealth;
    private int _playerCurrentHealth;
    // Start is called before the first frame update
    void Start()
    {
        _playerCurrentHealth = PlayerMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayerTakeDamage(int damageAmount)
    {
        _playerCurrentHealth -= damageAmount;

        if (_playerCurrentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
