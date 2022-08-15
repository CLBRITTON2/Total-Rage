using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class ObjectPoolItem
    {
        public GameObject pooledObject;
        public int setPoolAmount;
        public bool expandPool;
    }
}
