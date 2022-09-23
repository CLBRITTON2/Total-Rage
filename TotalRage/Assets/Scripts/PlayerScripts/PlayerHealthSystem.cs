using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    public int PlayerMaxHealth;
    private int _playerCurrentHealth;

    UICanvasController PlayerHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        _playerCurrentHealth = PlayerMaxHealth;

        PlayerHealthBar = FindObjectOfType<UICanvasController>();
        PlayerHealthBar.SetPlayerMaxHealth(PlayerMaxHealth);
    }
    public void PlayerTakeDamage(int damageAmount)
    {
        _playerCurrentHealth -= damageAmount;
        PlayerHealthBar.SetPlayerHealthBar(_playerCurrentHealth);

        if (_playerCurrentHealth <= 0)
        {
            gameObject.SetActive(false);
            FindObjectOfType<GameManager>().PlayerRespawn();
        }
    }
    public void HealPlayer(int amountToHeal)
    {
        _playerCurrentHealth += amountToHeal;

        if (_playerCurrentHealth > PlayerMaxHealth)
        {
            _playerCurrentHealth = PlayerMaxHealth;           
        }

        PlayerHealthBar.SetPlayerHealthBar(_playerCurrentHealth);
    }
}
