using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    private const string assetBundlePath = "Assets/AssetBundles/WakAsset";
    private AssetBundle assetBundle;

    private Dictionary<string, GameObject> _loadedObjectDic = new Dictionary<string, GameObject>();

    public GameObject LoadPrefab(string path)
    {
        if (assetBundle == null)
        {
            assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
        }

        if (_loadedObjectDic.ContainsKey(path))
        {
            return _loadedObjectDic[path];
        }

         var prefab = assetBundle.LoadAsset<GameObject>(path);

        _loadedObjectDic[path] = prefab;

        return prefab;
    }
}
