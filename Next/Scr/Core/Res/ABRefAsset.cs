using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace SkySwordKill.Next.Res;

public class ABRefAsset
{
    public string ABPath { get; set; }
    public AssetBundle RefAssetBundle { get; set; }

    public ABRefAsset(string abPath)
    {
        ABPath = abPath;
    }
    
    public void LoadAssetBundle()
    {
        UnloadAssetBundle();
        RefAssetBundle = AssetBundle.LoadFromFile(ABPath);
    }
    
    public void UnloadAssetBundle()
    {
        if (RefAssetBundle != null)
        {
            RefAssetBundle.Unload(false);
            RefAssetBundle = null;
        }
    }

    public bool HasAsset(string resPath)
    {
        return RefAssetBundle.Contains(resPath);
    }

    public Object Load(string resPath)
    {
        var asset = RefAssetBundle.LoadAsset(resPath);
        return asset;
    }

    public async UniTask<Object> LoadAsync(string resPath)
    {
        var request = RefAssetBundle.LoadAssetAsync(resPath);
        await request;
        return request.asset;
    }
}