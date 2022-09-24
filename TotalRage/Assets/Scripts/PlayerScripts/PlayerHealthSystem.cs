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
        AudioManager.Instance.PlaySound("PlayerTakeDamage");
        _playerCurrentHealth -= damageAmount;
        PlayerHealthBar.SetPlayerHealthBar(_playerCurrentHealth);

        if (_playerCurrentHealth <= 0)
        {
            AudioManager.Instance.PlaySound("PlayerDead");
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
