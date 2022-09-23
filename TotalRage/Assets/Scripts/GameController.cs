using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float XPos;
    public float YPos;
    public float ZPos;
    public static GameController Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }
    IEnumerator SpawnEnemies()
    {
        XPos = Random.Range(-22f, 22f);
        YPos = Random.Range(0f, 0f);
        ZPos = Random.Range(22f, -22f);
        Vector3 spawnPosition = new Vector3(XPos, YPos, ZPos);
        Quaternion spawnRotation = Quaternion.identity;
        ObjectPoolManager.Instance.SpawnFromObjectPool("Melee Enemy", spawnPosition, spawnRotation);
        ObjectPoolManager.Instance.SpawnFromObjectPool("Robot Enemy", spawnPosition, spawnRotation);
        yield return new WaitForSeconds(0.1f);
    }
}
