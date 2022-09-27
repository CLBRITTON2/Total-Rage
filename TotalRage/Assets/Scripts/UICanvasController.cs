using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UICanvasController : MonoBehaviour
{
    public TextMeshProUGUI AmmoInfoText, PlayersTotalAmmoText, WaveText, PointsText;
    public Slider HealthSlider;
    
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
