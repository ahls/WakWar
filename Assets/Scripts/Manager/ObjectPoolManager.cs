using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public ObjectPoolManager(GameObject disable)
    {
        _disable = disable;
    }

    private GameObject _disable;
    private Dictionary<string, ObjectPoolData> _objectPoolDic = new Dictionary<string, ObjectPoolData>();
    private Dictionary<string, ObjectPoolData> _globalObjectPoolDic = new Dictionary<string, ObjectPoolData>();

    public void ObjectPooling(string path, GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.parent = _disable.transform;

        if (!_objectPoolDic.TryGetValue(path, out ObjectPoolData objectData))
        {
            objectData = new ObjectPoolData();
            _objectPoolDic.Add(path, objectData);
        }

        objectData.PoolingObject(gameObject);
    }

    public void GlobalObjectPooling(string path, GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.parent = _disable.transform;


        if (!_globalObjectPoolDic.TryGetValue(path, out ObjectPoolData objectData))
        {
            objectData = new ObjectPoolData();
            _globalObjectPoolDic.Add(path, objectData);
        }

        objectData.PoolingObject(gameObject);
    }
    
    public GameObject GetObject(string path)
    {
        if (_objectPoolDic.ContainsKey(path))
            {
                var pooledObjectData = _objectPoolDic[path];

            if (pooledObjectData.GetPoolObjectCount() > 0)
            {
                return pooledObjectData.GetObject();
            }
        }

        if (_globalObjectPoolDic.ContainsKey(path))
            {
                var pooledObjectData = _globalObjectPoolDic[path];

            if (pooledObjectData.GetPoolObjectCount() > 0)
            {
                return pooledObjectData.GetObject();
            }
        }

        return null;
    }
    
    public void ResetObjectPool()
    {
        foreach(var destoryObject in _objectPoolDic)
        {
            while (destoryObject.Value.GetPoolObjectCount() > 0)
            {
                Destroy(destoryObject.Value.GetObject());
            }
        }
    
        _objectPoolDic.Clear();
    }

    public GameObject CreatObject(GameObject _gameObject)
    {
        return Instantiate(_gameObject);
    }
}
