using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkySwordKill.Next;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SkySwordKill.Next.Res;

public class ResourcesManager : MonoBehaviour
{
    public delegate void FileHandle(string virtualPath, string filePath);
        
    #region 字段

    public Dictionary<string, FileAsset> fileAssets = new Dictionary<string, FileAsset>(StringComparer.OrdinalIgnoreCase);
    public Dictionary<int, Sprite> spriteCache = new Dictionary<int, Sprite>();

    #endregion

    #region 属性



    #endregion

    #region 回调方法



    #endregion

    #region 公共方法
        
    public void Init()
    {
        LoadInnerAsset();
    }
        
    public void Reset()
    {
        fileAssets.Clear();
        spriteCache.Clear();
        LoadInnerAsset();
    }

    public void LoadInnerAsset()
    {
        CacheAssetDir(Main.PathInnerAssetDir.Value);
    }
        
    public void CacheAssetDir(string rootPath)
    {
        DirectoryHandle("Assets", rootPath, AddAsset);
    }

    public void DirectoryHandle(string rootPath,string dirPath,FileHandle fileHandle)
    {
        if(!Directory.Exists(dirPath))
            return;

        foreach (var directory in Directory.GetDirectories(dirPath))
        {
            var name = Path.GetFileNameWithoutExtension(directory);
            DirectoryHandle($"{rootPath}/{name}", directory, fileHandle);
        }

        foreach (var file in Directory.GetFiles(dirPath))
        {
            var fileName = Path.GetFileName(file);
                
            var cachePath = $"{rootPath}/{fileName}";
            fileHandle.Invoke(cachePath, file);
        }
    }

    /// <summary>
    /// 异步加载资源，返回值表示资源是否存在
    /// </summary>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public bool TryGetAsset(string path,Action<Object> callback)
    {
        if (fileAssets.TryGetValue(path, out var fileAsset))
        {
            fileAsset.LoadAssetAsync(callback);
            return true;
        }
        return false;
    }
        
    /// <summary>
    /// 同步加载资源，返回值表示资源是否存在且加载完毕
    /// </summary>
    /// <param name="path"></param>
    /// <param name="asset"></param>
    /// <returns></returns>
    public bool TryGetAsset(string path,out Object asset)
    {
        if (fileAssets.TryGetValue(path, out var fileAsset))
        {
            asset = fileAsset.LoadAsset();
            if (asset != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        asset = null;
        return false;
    }
        
    /// <summary>
    /// 获取FileAsset，可以通过FileAsset获取资源原始位置及加载信息等
    /// </summary>
    /// <param name="path"></param>
    /// <param name="asset"></param>
    /// <returns></returns>
    public bool TryGetFileAsset(string path,out FileAsset fileAsset)
    {
        if (fileAssets.TryGetValue(path, out fileAsset))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 异步按文件夹加载资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    /// <param name="includeSubfolder">是否包含子文件夹</param>
    /// <param name="extension">文件拓展名</param>
    /// <returns></returns>
    public void TryGetAssets(string path,Action<Object[]> callback,bool includeSubfolder = false,string extension = ".*")
    {
        var list = new List<FileAsset>();
        var transPath = path.ToLower();
        foreach (var pair in fileAssets)
        {
            var curPath = pair.Key;
            var asset = pair.Value;
            if (curPath.StartsWith(transPath)
                && (includeSubfolder || !curPath.Substring(transPath.Length).Contains("/")) 
                && (extension == ".*" || curPath.EndsWith(extension)))
            {
                list.Add(asset);
            }
        }

        StartCoroutine(LoadAssets(list,callback));
    }
        
    /// <summary>
    /// 同步按文件夹加载资源
    /// 如果一个资源没有准备就绪，不会被加载进来
    /// </summary>
    /// <param name="path"></param>
    /// <param name="asstes"></param>
    /// <param name="includeSubfolder"></param>
    /// <param name="extension"></param>
    public void TryGetAssets(string path,out Object[] asstes,bool includeSubfolder = false,string extension = ".*")
    {
        var list = new List<FileAsset>();
        var transPath = path.ToLower();
        foreach (var pair in fileAssets)
        {
            var curPath = pair.Key;
            var asset = pair.Value;
            if (curPath.StartsWith(transPath)
                && (includeSubfolder || !curPath.Substring(transPath.Length).Contains("/")) 
                && (extension == ".*" || curPath.EndsWith(extension)))
            {
                list.Add(asset);
            }
        }

        asstes = list
            .Where(asset=>asset.IsDone)
            .Select(asset=>asset.LoadAsset())
            .ToArray();
    }

    private IEnumerator LoadAssets(IEnumerable<FileAsset> assets,Action<Object[]> callback)
    {
        var loadAssets = new List<FileAsset>();
        var doneAssets = new List<FileAsset>();
            
        loadAssets.AddRange(assets);
            
        while (doneAssets.Count < loadAssets.Count)
        {
            for (int i = loadAssets.Count - 1; i >= 0; i--)
            {
                var asset = loadAssets[i];
                if (asset.IsDone)
                {
                    loadAssets.RemoveAt(i);
                    doneAssets.Add(asset);
                }
            }

            yield return null;
        }
            
        callback?.Invoke(doneAssets.Select(asset=>asset.asset).ToArray());
    }

    /// <summary>
    /// 是否存在资源（无论是否加载完毕）
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public bool HaveAsset(string path)
    {
        return fileAssets.ContainsKey(path);
    }
        
    /// <summary>
    /// 是否存在资源且加载完毕
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public bool IsAssetReady(string path)
    {
        return fileAssets.TryGetValue(path, out var fileAsset) && fileAsset.IsDone;
    }

    public void AddAsset(string cachePath,string path)
    {
        var fileAsset = new FileAsset(path);
        var key = cachePath.Replace(@"\\", "/").ToLower();
            
        if (fileAssets.TryGetValue(key, out var oldAsset))
        {
            Main.LogWarning($"重复添加Asset ({key})");
        }
        else
        {
            Main.LogInfo($"添加Asset ({key})");
        }
        fileAssets[key] = fileAsset;
    }

    public Sprite GetSpriteCache(Texture2D texture)
    {
        if(!spriteCache.TryGetValue(texture.GetHashCode(),out var sprite))
        {
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            spriteCache.Add(texture.GetHashCode(),sprite);
        }
        return sprite;
    }

    #endregion

    #region 私有方法

    private void DrawWindow(int id)
    {
        var assetCount = fileAssets.Count;
        var loadSuccessCount = fileAssets.Count(pair => pair.Value.IsDone);
        GUILayout.Label($"资源加载进度：{loadSuccessCount} / {assetCount}");
    }

    #endregion
}