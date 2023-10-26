using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace SkySwordKill.Next.Res;

public class FileResLoader : IResLoader
{
    public Dictionary<string, FileAsset> fileAssets = new Dictionary<string, FileAsset>(StringComparer.OrdinalIgnoreCase);

    public void AddFileAsset(string key, FileAsset fileAsset)
    {
        if(fileAssets.TryGetValue(key, out var oldFileAsset))
        {
            oldFileAsset.Unload();
        }
        
        fileAssets[key] = fileAsset;
    }

    public void Reset()
    {
        foreach (var fileAsset in fileAssets.Values)
        {
            fileAsset.Unload();
        }
        fileAssets.Clear();
    }

    public Object Load(string resPath)
    {
        if(fileAssets.TryGetValue(resPath, out var fileAsset))
        {
            return fileAsset.Load();
        }
        
        return null;
    }

    public async UniTask<Object> LoadAsync(string resPath)
    {
        if(fileAssets.TryGetValue(resPath, out var fileAsset))
        {
            return await fileAsset.LoadAsync();
        }
        
        return null;
    }

    public byte[] LoadBytes(string resPath)
    {
        if(fileAssets.TryGetValue(resPath, out var fileAsset))
        {
            var asset = fileAsset.Load();
            if (asset is BytesAsset bytesAsset)
            {
                return bytesAsset.Bytes;
            }
            else
            {
                var type = asset == null ? "null" : asset.GetType().ToString();
                Main.LogError($"FileAsset:{resPath} 不是字节流资源，Type:{type}");
            }
        }
        
        return null;
    }

    public async UniTask<byte[]> LoadBytesAsync(string resPath)
    {
        if(fileAssets.TryGetValue(resPath, out var fileAsset))
        {
            var asset = await fileAsset.LoadAsync();
            if (asset is BytesAsset bytesAsset)
            {
                return bytesAsset.Bytes;
            }
            else
            {
                var type = asset == null ? "null" : asset.GetType().ToString();
                Main.LogError($"FileAsset:{resPath} 不是字节流资源，Type:{type}");
            }
        }
        
        return null;
    }

    public string LoaderCode => "File";
}