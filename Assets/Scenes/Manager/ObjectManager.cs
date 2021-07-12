using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    public GameObject SpawnObject(string path, bool isUI = false)
    {
        return Global.ResourceManager.LoadPrefab(path, isUI);
    }

    public void ReleaseObject(string path, GameObject gameObject)
    {
        Global.ObjectPoolManager.ObjectPooling(path, gameObject);
    }

    public void ReleaseGlobalObject(string path, GameObject gameObject)
    {
        Global.ObjectPoolManager.GlobalObjectPooling(path, gameObject);
    }

    public void ReleaseCanvasObject(string path, GameObject gameObject)
    {
        Global.ObjectPoolManager.CanvasObjectPooling(path, gameObject);
    }

    public void ReleaseCanvasGlobalObject(string path, GameObject gameObject)
    {
        Global.ObjectPoolManager.CanvasGlobalObjectPooling(path, gameObject);
    }
}
