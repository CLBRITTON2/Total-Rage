using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Pool
{
    public string Tag;
    public GameObject Prefab;
    public int PoolSize;
}
public class ObjectPoolManager : MonoBehaviour
{
    public Dictionary<string, Queue<GameObject>> PoolDictionary;
    public List<Pool> Pools;
    public static ObjectPoolManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.PoolSize; i++)
            {
                GameObject pooledObject = Instantiate(pool.Prefab);
                pooledObject.SetActive(false);
                objectPool.Enqueue(pooledObject);
            }
            PoolDictionary.Add(pool.Tag, objectPool);
        }
    }
    public GameObject SpawnFromObjectPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            throw new ArgumentException($"Pool with tag {tag} doesn't exist");
        }

        GameObject objectToSpawn = PoolDictionary[tag].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        PoolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
