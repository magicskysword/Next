using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkySwordKill.Next.Res;

public class ABResLoader : IResLoader
{
    /// <summary>
    /// ab引用列表，优先级高的在前面
    /// </summary>
    public List<ABRefAsset> abRefAssets = new List<ABRefAsset>();
    
    public void AddABAssetToFirst(ABRefAsset abAsset)
    {
        abRefAssets.Insert(0, abAsset);
    }
    
    public void Reset()
    {
        foreach (var abRefAsset in abRefAssets)
        {
            abRefAsset.UnloadAssetBundle();
        }
        abRefAssets.Clear();
    }

    public Object Load(string resPath)
    {
        foreach (var abRef in abRefAssets)
        {
            if(abRef.HasAsset(resPath))
            {
                return abRef.Load(resPath);
            }
        }

        return null;
    }

    public async UniTask<Object> LoadAsync(string resPath)
    {
        foreach (var abRef in abRefAssets)
        {
            if(abRef.HasAsset(resPath))
            {
                return await abRef.LoadAsync(resPath);
            }
        }

        return null;
    }

    public byte[] LoadBytes(string resPath)
    {
        foreach (var abRef in abRefAssets)
        {
            if(abRef.HasAsset(resPath))
            {
                var asset = abRef.Load(resPath);
                if (asset is TextAsset textAsset)
                {
                    return textAsset.bytes;
                }
            }
        }

        return null;
    }

    public async UniTask<byte[]> LoadBytesAsync(string resPath)
    {
        foreach (var abRef in abRefAssets)
        {
            if(abRef.HasAsset(resPath))
            {
                var asset = await abRef.LoadAsync(resPath);
                if (asset is TextAsset textAsset)
                {
                    return textAsset.bytes;
                }
            }
        }

        return null;
    }

    public string LoaderCode => "AB";
}