using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResourceManager
{
    private string assetBundlePath = "Assets/StreamingAssets/WakAsset";
    private AssetBundle assetBundle;

    private Dictionary<string, GameObject> _loadedObjectDic = new Dictionary<string, GameObject>();
    private Dictionary<string, Sprite> _loadedTexture = new Dictionary<string, Sprite>();

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
            preloadTextures();
        }
    }

    public GameObject LoadPrefab(string path, bool isUI = false)
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

            return Global.ObjectPoolManager.CreatObject(_loadedObjectDic[path], isUI);
        }

        var prefab = assetBundle.LoadAsset<GameObject>(path);

        _loadedObjectDic[path] = prefab;

        return Global.ObjectPoolManager.CreatObject(prefab, isUI);
    }

    public Sprite LoadTexture(string path, bool isUI = false)
    {
        if (assetBundle == null)
        {
            throw new Exception("AseetBundle is Null");
        }

        return _loadedTexture[path];

    }
    private void preloadTextures()
    {
        
        foreach (var path in assetBundle.GetAllAssetNames())
        {
            Debug.Log(path);
            Sprite[] _sprites;
            if(path.Contains("sprites/weapon/img_"))//빌드에서도 적용 되는지 확인 필요
            {
                Debug.Log("worked!!");
                _sprites = assetBundle.LoadAssetWithSubAssets<Sprite>(path);
                foreach (var subsprite in _sprites)
                {
                    _loadedTexture[subsprite.name] = subsprite;
                }
            }
        }

    }
}
