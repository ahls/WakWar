using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    private const string assetBundlePath = "";
    private AssetBundle assetBundle;

    private Dictionary<string, GameObject> _loadedObjectDic;

    public GameObject LoadPrefab(string path)
    {
        assetBundle = AssetBundle.LoadFromFile(assetBundlePath);

        if (_loadedObjectDic.ContainsKey(path))
        {
            return _loadedObjectDic[path];
        }

         var prefab = assetBundle.LoadAsset<GameObject>(path);

        _loadedObjectDic[path] = prefab;

        return prefab;
    }
}
