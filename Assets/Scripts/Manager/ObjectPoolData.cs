using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolData
{
    private Queue<GameObject> gameObjectsQueue = new Queue<GameObject>();

    public GameObject GetObject()
    {
        return gameObjectsQueue.Dequeue();
    }
    
    public void PoolingObject(GameObject poolObject)
    {
        gameObjectsQueue.Enqueue(poolObject);
    }

    public int GetPoolObjectCount()
    {
        return gameObjectsQueue.Count;
    }
}
