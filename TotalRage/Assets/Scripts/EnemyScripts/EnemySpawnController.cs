using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public static EnemySpawnController Instance;
    private UICanvasController _playerDataUIController;
    public GameObject[] EnemyTypes;
    public GameObject[] EnemySpawnPoints;
    public float SpawnArea = 1.5f;
    public float TimeBetweenWaves = 10.0f;
    public float TimeBetweenEnemySpawn = 0.5f;
    public int Wave = 1;
    public int StartEnemyCount = 3;
    public int EnemiesToAddPerWave = 2;
    public int ActiveEnemyCount = 0;
    private int _enemiesSpawned = 0;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _playerDataUIController = FindObjectOfType<UICanvasController>();
        UpdateWaveInfoText();
        EnemySpawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        Invoke("StartWave", TimeBetweenWaves);
    }
    void StartWave()
    {
        InvokeRepeating("SpawnEnemy", TimeBetweenEnemySpawn, TimeBetweenEnemySpawn);
    }
    private void EndWave()
    {
        Wave++;
        UpdateWaveInfoText();
        ActiveEnemyCount = 0;
        _enemiesSpawned = 0;
        Invoke("StartWave", TimeBetweenWaves);
    }
    public void DecreaseActiveEnemyCount()
    {
        ActiveEnemyCount--;
        CheckActiveEnemyCount();
    }
    void CheckActiveEnemyCount()
    {
        if (ActiveEnemyCount == 0)
        {
            EndWave();
        }
    }
    void SpawnEnemy()
    {
        int randomSpawnpointNumber = Random.Range(0, EnemySpawnPoints.Length - 1);
        Transform spawnPoint = EnemySpawnPoints[randomSpawnpointNumber].transform;

        float spawnXPos = spawnPoint.transform.position.x + Random.Range(-SpawnArea, SpawnArea);
        float spawnYPos = spawnPoint.transform.position.y + Random.Range(-SpawnArea, SpawnArea);
        float spawnZPos = spawnPoint.transform.position.z + Random.Range(-SpawnArea, SpawnArea);

        int randomEnemyNumber = Random.Range(0, EnemyTypes.Length);
        Vector3 spawnPosition = new Vector3(spawnXPos, spawnYPos, spawnZPos);
        Quaternion spawnRotation = Quaternion.identity;

        Instantiate(EnemyTypes[randomEnemyNumber], spawnPosition, spawnRotation);
        _enemiesSpawned++;
        ActiveEnemyCount++;

        if (_enemiesSpawned >= (StartEnemyCount + Wave * EnemiesToAddPerWave))
        {
            CancelInvoke("SpawnEnemy");
        }
    }
    private void UpdateWaveInfoText()
    {
        _playerDataUIController.WaveText.SetText($"WAVE {Wave}");
    }
}
