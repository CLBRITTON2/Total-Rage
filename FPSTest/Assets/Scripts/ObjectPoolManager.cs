using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    public GameObject pooledObject;
    public int pooledAmount;
    public bool expandPool;

    private List<GameObject> pooledObjectList;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pooledObjectList = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject gameObject = Instantiate(pooledObject);
            gameObject.SetActive(false);
            pooledObjectList.Add(gameObject);
        }
    }
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < pooledObjectList.Count; i++)
        {
            if (!pooledObjectList[i].activeInHierarchy)
            {
                return pooledObjectList[i];
            }
        }

        if(expandPool)
        {
            GameObject gameObject = Instantiate(pooledObject);
            gameObject.SetActive(false);
            pooledObjectList.Add(pooledObject);
            return gameObject;
        }
        return null;
    }
}
