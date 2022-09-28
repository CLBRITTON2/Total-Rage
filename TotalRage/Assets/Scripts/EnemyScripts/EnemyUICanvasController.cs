using UnityEngine;
using UnityEngine.UI;

public class EnemyUICanvasController : MonoBehaviour
{
    public Slider EnemyHealthSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void SetEnemyMaxHealth(int enemyHealthValue)
    {
        EnemyHealthSlider.maxValue = enemyHealthValue;
        EnemyHealthSlider.value = enemyHealthValue;
    }
    public void SetEnemyHealthBar(int enemyHealthValue)
    {
        EnemyHealthSlider.value = enemyHealthValue;
    }
}
