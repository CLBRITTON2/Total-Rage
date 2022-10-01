using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEventSceneLoader : MonoBehaviour
{
    public void LoadLevelOne()
    {
        LoadScene.Instance.LoadNextScene("LevelOne");
    }
    public void LoadLevelTwo()
    {
        LoadScene.Instance.LoadNextScene("LevelTwo");
    }
    public void LoadMainMenu()
    {
        LoadScene.Instance.LoadNextScene("MainMenuScene");
    }
}
