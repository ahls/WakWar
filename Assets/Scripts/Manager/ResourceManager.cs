using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResourceManager
{
    private string _assetBundlePath = "Assets/StreamingAssets/WakAsset";
    private AssetBundle _assetBundle;

    private Dictionary<string, GameObject> _loadedObjectDic = new Dictionary<string, GameObject>();
    private Dictionary<string, Sprite> _loadedTexture = new Dictionary<string, Sprite>();

    public ResourceManager()
    {
#if UNITY_EDITOR
        _assetBundlePath = "Assets/StreamingAssets/WakAsset";
#else
        _assetBundlePath = Application.dataPath + "/StreamingAssets/WakAsset";
#endif
        if (_assetBundle == null)
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(_assetBundlePath));
            _assetBundle = request.assetBundle;
            //AssetBundle.LoadFromFile(assetBundlePath);
            PreloadTextures();
        }
    }

    public GameObject LoadPrefab(string path, bool isUI = false)
    {
        if (_assetBundle == null)
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

        var prefab = _assetBundle.LoadAsset<GameObject>(path);

        _loadedObjectDic[path] = prefab;

        return Global.ObjectPoolManager.CreatObject(prefab, isUI);
    }

    public Sprite LoadTexture(string path, bool isUI = false)
    {
        if (_assetBundle == null)
        {
            throw new Exception("AseetBundle is Null");
        }

        return _loadedTexture[path];

    }

    private void PreloadTextures()
    {
        foreach (var path in _assetBundle.GetAllAssetNames())
        {
            Sprite[] sprites;
            if(path.Contains("sprites/weapon/img_"))//빌드에서도 적용 되는지 확인 필요
            {
                sprites = _assetBundle.LoadAssetWithSubAssets<Sprite>(path);
                foreach (var subsprite in sprites)
                {
                    _loadedTexture[subsprite.name] = subsprite;
                }
            }
        }

    }
}
