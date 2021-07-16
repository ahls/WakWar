using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolData
{
    private Queue<GameObject> _gameObjectsQueue = new Queue<GameObject>();

    public GameObject GetObject()
    {
        var returnObject = _gameObjectsQueue.Dequeue();
        returnObject.transform.SetParent(null,false);
        returnObject.SetActive(true);

        return returnObject;
    }
    
    public void PoolingObject(GameObject poolObject)
    {
        _gameObjectsQueue.Enqueue(poolObject);
    }

    public int GetPoolObjectCount()
    {
        return _gameObjectsQueue.Count;
    }
}
