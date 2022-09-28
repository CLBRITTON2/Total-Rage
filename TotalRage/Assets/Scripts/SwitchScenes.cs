using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    public void EasyModeButtonSelected()
    {
        SceneManager.LoadScene("LevelOne");
    }
    public void HardModeButtonSelected()
    {
        SceneManager.LoadScene("LevelTwo");
    }
}
