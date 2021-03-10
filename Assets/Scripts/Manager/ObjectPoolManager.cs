using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private Dictionary<string, GameObject> _objectPoolDic;
    private Dictionary<string, GameObject> _globalObjectPoolDic;

    public void ObjectPooling(string path, GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.parent = this.transform;
        _objectPoolDic.Add(path, gameObject);
    }

    public void GlobalObjectPooling(string path, GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.parent = this.transform;
        _globalObjectPoolDic.Add(path, gameObject);
    }

    public GameObject GetObject(string path)
    {
        if (_objectPoolDic.ContainsKey(path))
        {
            var pooledObject = _objectPoolDic[path];
            _objectPoolDic.Remove(path);

            return pooledObject;
        }

        return null;
    }

    public GameObject GetGlobalObject(string path)
    {
        if (_globalObjectPoolDic.ContainsKey(path))
        {
            var pooledObject = _globalObjectPoolDic[path];
            _globalObjectPoolDic.Remove(path);

            return pooledObject;
        }

        return null;
    }

    public void ResetObjectPool()
    {
        foreach(var destoryObject in _objectPoolDic)
        {
            Destroy(destoryObject.Value);
        }

        _objectPoolDic.Clear();
    }
}
