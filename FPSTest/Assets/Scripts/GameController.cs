using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float xPos;
    public float yPos;
    public float zPos;
    public static GameController instance;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }
    IEnumerator SpawnEnemies()
    {
        xPos = Random.Range(-22f, 22f);
        yPos = Random.Range(0f, 10f);
        zPos = Random.Range(22f, -22f);
        Vector3 spawnPosition = new Vector3(xPos, yPos, zPos);
        Quaternion spawnRotation = Quaternion.identity;
        ObjectPoolManager.instance.SpawnFromObjectPool("Melee Enemy", spawnPosition, spawnRotation);
        ObjectPoolManager.instance.SpawnFromObjectPool("Robot Enemy", spawnPosition, spawnRotation);
        yield return new WaitForSeconds(0.1f);
    }
}
