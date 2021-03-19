using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResourceManager
{
    private string assetBundlePath = "Assets/StreamingAssets/WakAsset";
    private AssetBundle assetBundle;

    private Dictionary<string, GameObject> _loadedObjectDic = new Dictionary<string, GameObject>();

    public ResourceManager()
    {
#if UNITY_EDITOR
        assetBundlePath = "Assets/StreamingAssets/WakAsset";
#else
        assetBundlePath = Application.dataPath + "/StreamingAssets/WakAsset";
#endif
        if (assetBundle == null)
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(assetBundlePath));
            assetBundle = request.assetBundle;
            //AssetBundle.LoadFromFile(assetBundlePath);
        }
    }

    public GameObject LoadPrefab(string path)
    {
        if (assetBundle == null)
        {
            throw new Exception("AseetBundle is Null");
        }
       
        if (_loadedObjectDic.ContainsKey(path))
        {
            var pooledObject = Global.ObjectPoolManager.GetObject(path);

            if (pooledObject != null)
            {
                return pooledObject;
            }

            return _loadedObjectDic[path];
        }

        var prefab = assetBundle.LoadAsset<GameObject>(path);

        _loadedObjectDic[path] = prefab;
        
        return prefab;
    }
}
