using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private float _playerRespawnTime = 3f;

    public void PlayerRespawn()
    {
        StartCoroutine(PlayerRespawnTimer());
    }

    IEnumerator PlayerRespawnTimer()
    {
        yield return new WaitForSeconds(_playerRespawnTime);

        SceneManager.LoadScene("PlayGround");
    }
}
