using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UICanvasController : MonoBehaviour
{
    public TextMeshProUGUI AmmoInfoText, PlayersTotalAmmoText;
    public Slider HealthSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetPlayerMaxHealth(int healthValue)
    {
        HealthSlider.maxValue = healthValue;
        HealthSlider.value = healthValue;
    }
    public void SetPlayerHealthBar(int healthValue)
    {
        HealthSlider.value = healthValue;
    }
}
