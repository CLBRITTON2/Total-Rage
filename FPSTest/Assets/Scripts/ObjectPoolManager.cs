using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    private List<GameObject> pooledObjectList;
    public List<ObjectPoolItem> itemsToPool;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {    
        pooledObjectList = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.setPoolAmount; i++)
            {
                GameObject gameObject = Instantiate(item.pooledObject);
                gameObject.SetActive(false);
                pooledObjectList.Add(gameObject);
            }
        }
    }
    public GameObject GetPooledObject(string tag)
    {
        for(int i = 0; i < pooledObjectList.Count; i++)
        {
            if (!pooledObjectList[i].activeInHierarchy && pooledObjectList[i].tag == tag)
            {
                return pooledObjectList[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.pooledObject.tag == tag)
            {
                if (item.expandPool)
                {
                    GameObject gameObject = Instantiate(item.pooledObject);
                    gameObject.SetActive(false);
                    pooledObjectList.Add(gameObject);
                    return gameObject;
                }
            }
        }
        return null;
    }
}
