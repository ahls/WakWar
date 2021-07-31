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
    private Dictionary<string,AudioClip> _loadedAudio = new Dictionary<string, AudioClip>();

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
            PreloadResources();
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

    public Sprite LoadTexture(string path)
    {
        if (_assetBundle == null)
        {
            throw new Exception("AseetBundle is Null");
        }
        else if (!_loadedTexture.ContainsKey(path))
        {
            Debug.Log(path + " does not exist in the textures.");
            return null;
        }
        return _loadedTexture[path];

    }
    public AudioClip LoadAudio(string path)
    {
        if (_assetBundle == null)
        {
            throw new Exception("AssetBundle is Null");
        }
        else if (!_loadedAudio.ContainsKey(path))
        {
            return null;
        }
        return _loadedAudio[path];
    }


    private void PreloadResources()
    {
        foreach (var path in _assetBundle.GetAllAssetNames())
        {
            if (path.Contains("images/"))//빌드에서도 적용 되는지 확인 필요
            {
                Sprite[] sprites;
                Debug.Log("텍스쳐: "+path + " 을 불러오는중입니다.");
                sprites = _assetBundle.LoadAssetWithSubAssets<Sprite>(path);
                foreach (var subsprite in sprites)
                {
                    _loadedTexture[subsprite.name] = subsprite;
                }
            }
            else if(path.Contains("audio/"))
            {
                AudioClip audioClip = _assetBundle.LoadAsset<AudioClip>(path);
                _loadedAudio[audioClip.name] = audioClip;
            }
        }
    }


}
