using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    public int PlayerMaxHealth;
    private int _playerCurrentHealth;
    public Animator PlayerUIAnimator;

    UICanvasController PlayerUICanvas;

    void Start()
    {
        _playerCurrentHealth = PlayerMaxHealth;

        PlayerUICanvas = FindObjectOfType<UICanvasController>();
        PlayerUICanvas.SetPlayerMaxHealth(PlayerMaxHealth);
    }
    public void PlayerTakeDamage(int damageAmount)
    {
        if (_playerCurrentHealth > 0)
        {
            PlayerUIAnimator.SetTrigger("PlayerTakeDamage");
            AudioManager.Instance.PlaySoundOneShot("PlayerTakeDamage");
            _playerCurrentHealth -= damageAmount;
            PlayerUICanvas.SetPlayerHealthBar(_playerCurrentHealth);
            PlayerUIAnimator.SetBool("PlayerTakingDamage", false);
        }

        if (_playerCurrentHealth <= 0)
        {
            AudioManager.Instance.PlaySoundOneShot("PlayerDead");
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

        PlayerUICanvas.SetPlayerHealthBar(_playerCurrentHealth);
    }
}
