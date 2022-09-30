using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private UICanvasController _playerDataUIController;
    private float _playerRespawnTime = 3f;
    public static int PlayerPoints = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one game manager in scene.");
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        _playerDataUIController = FindObjectOfType<UICanvasController>();
    }
    private void Update()
    {
        UpdatePointsText();
    }
    public void PlayerRespawn()
    {
        StartCoroutine(PlayerRespawnTimer());
    }

    IEnumerator PlayerRespawnTimer()
    {
        yield return new WaitForSeconds(_playerRespawnTime);

        SceneManager.LoadScene("MainMenuScene");
    }
    private void UpdatePointsText()
    {
        _playerDataUIController.PointsText.SetText($"POINTS: {PlayerPoints}");
    }
}
