using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseScreenUI;
    public static bool GameIsPaused = false;

    private void Pause()
    {
        Time.timeScale = 0f;
        PauseScreenUI.SetActive(true);
        GameIsPaused = true;

        Cursor.lockState = CursorLockMode.None;
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        PauseScreenUI.SetActive(false);
        GameIsPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // You can exit the pause menu by hitting escape or the resume button
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("MainMenuScene");
    }
    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
