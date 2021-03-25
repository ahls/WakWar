using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public ObjectPoolManager(GameObject disable, GameObject disableCanvas)
    {
        _disable = disable;
        _disableCanvas = disableCanvas;
    }

    private GameObject _disable;
    private GameObject _disableCanvas;

    private Dictionary<string, ObjectPoolData> _canvasObjectPoolDic = new Dictionary<string, ObjectPoolData>();
    private Dictionary<string, ObjectPoolData> _canvasGlobalObjectPoolDic = new Dictionary<string, ObjectPoolData>();

    private Dictionary<string, ObjectPoolData> _objectPoolDic = new Dictionary<string, ObjectPoolData>();
    private Dictionary<string, ObjectPoolData> _globalObjectPoolDic = new Dictionary<string, ObjectPoolData>();

    public void CanvasObjectPooling(string path, GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.parent = _disableCanvas.transform;

        if (!_canvasObjectPoolDic.TryGetValue(path, out ObjectPoolData objectData))
        {
            objectData = new ObjectPoolData();
            _canvasObjectPoolDic.Add(path, objectData);
        }

        objectData.PoolingObject(gameObject);
    }

    public void CanvasGlobalObjectPooling(string path, GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.parent = _disableCanvas.transform;

        if (!_canvasGlobalObjectPoolDic.TryGetValue(path, out ObjectPoolData objectData))
        {
            objectData = new ObjectPoolData();
            _canvasGlobalObjectPoolDic.Add(path, objectData);
        }

        objectData.PoolingObject(gameObject);
    }

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

        if (_canvasObjectPoolDic.ContainsKey(path))
        {
            var pooledObjectData = _canvasObjectPoolDic[path];

            if (pooledObjectData.GetPoolObjectCount() > 0)
            {
                return pooledObjectData.GetObject();
            }
        }

        if (_canvasGlobalObjectPoolDic.ContainsKey(path))
        {
            var pooledObjectData = _canvasGlobalObjectPoolDic[path];

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

        foreach (var destoryObject in _canvasObjectPoolDic)
        {
            while (destoryObject.Value.GetPoolObjectCount() > 0)
            {
                Destroy(destoryObject.Value.GetObject());
            }
        }

        _canvasObjectPoolDic.Clear();
    }

    public GameObject CreatObject(GameObject _gameObject, bool isCanvas)
    {
        if (isCanvas)
        {
            return Instantiate(_gameObject, _disableCanvas.transform);
        }
       
        return Instantiate(_gameObject, _disable.transform);
    }
}
