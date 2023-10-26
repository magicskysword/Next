using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using SkySwordKill.Next.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace SkySwordKill.Next.Res;

public class ResourcesManager : MonoBehaviour
{
    public Dictionary<string, ResMapper> resMappers = new Dictionary<string, ResMapper>(StringComparer.OrdinalIgnoreCase);
    
    public Dictionary<int, Sprite> spriteCache = new Dictionary<int, Sprite>();
    
    public FileResLoader FileResLoader { get; } = new FileResLoader();
    
    public ABResLoader ABResLoader { get; } = new ABResLoader();
    
    public IEnumerable<IResLoader> ResLoaders
    {
        get
        {
            yield return FileResLoader;
            yield return ABResLoader;
        }
    }
    
    #region 管理器接口

    public void Init()
    {
        Reset();
    }
        
    public void Reset()
    {
        // 清理Sprite缓存
        foreach (var pair in spriteCache)
        {
            Destroy(pair.Value);
        }
        spriteCache.Clear();
        
        // 清理资源缓存
        foreach (var resLoader in ResLoaders)
        {
            resLoader.Reset();
        }
        
        Resources.UnloadUnusedAssets();
        
        // 加载内部资源
        LoadInnerAsset();
    }

    public void LoadInnerAsset()
    {
        CacheAssetDir(Main.PathInnerAssetDir.Value, true);
    }
        
    public void CacheAssetDir(string rootPath, bool dontDestroyOnLoad = false)
    {
        if (!Directory.Exists(rootPath))
        {
            return;
        }
        
        FileUtils.DirectoryHandle("Assets", rootPath, 
            ((path, filePath) => AddFileAsset(path, filePath, dontDestroyOnLoad)));
    }
    
    public void CacheAssetBundleDir(string rootPath)
    {
        if (!Directory.Exists(rootPath))
        {
            return;
        }
        
        var abPaths = Directory.GetFiles(rootPath, "*.ab", SearchOption.AllDirectories);
        foreach (var abPath in abPaths)
        {
            AddABAsset(abPath);
        }
    }

    #endregion

    #region 资源加载接口

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public async UniTask<Object> LoadAssetAsync(string path)
    {
        if (resMappers.TryGetValue(path, out var resMapper))
        {
            return await resMapper.LoadAsync();
        }

        return null;
    }
    
    /// <summary>
    /// 异步加载资源，泛型接口
    /// </summary>
    /// <param name="path"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async UniTask<T> LoadAssetAsync<T>(string path) where T : Object
    {
        return await LoadAssetAsync(path) as T;
    }

    /// <summary>
    /// 同步加载资源，返回值表示资源是否存在且加载完毕
    /// </summary>
    /// <param name="path"></param>
    /// <param name="asset"></param>
    /// <returns></returns>
    public Object LoadAsset(string path)
    {
        if (resMappers.TryGetValue(path, out var resMapper))
        {
            return resMapper.Load();
        }

        return null;
    }
    
    /// <summary>
    /// 同步加载泛型接口
    /// </summary>
    /// <param name="path"></param>
    /// <param name="asset"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T LoadAsset<T>(string path) where T : Object
    {
        return LoadAsset(path) as T;
    }
    
    public byte[] LoadBytes(string path)
    {
        if (resMappers.TryGetValue(path, out var resMapper))
        {
            return resMapper.LoadBytes();
        }
        
        return null;
    }
    
    public async UniTask<byte[]> LoadBytesAsync(string path)
    {
        if (resMappers.TryGetValue(path, out var resMapper))
        {
            return await resMapper.LoadBytesAsync();
        }
        
        return null;
    }
    
    public void LoadScene(string path, LoadSceneMode mode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(path, mode);
    }
    
    public async UniTask LoadSceneAsync(string path, LoadSceneMode mode = LoadSceneMode.Single)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(path, mode);
        await asyncOperation;
    }

    /// <summary>
    /// 是否存在资源
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public bool HaveAsset(string path)
    {
        return resMappers.ContainsKey(path);
    }

    /// <summary>
    /// 直接添加文件资源引用
    /// </summary>
    /// <param name="cachePath"></param>
    /// <param name="path"></param>
    /// <param name="dontDestroyOnLoad"></param>
    public void AddFileAsset(string cachePath,string path, bool dontDestroyOnLoad)
    {
        var fileAsset = new FileAsset(path);
        fileAsset.DontDestroyOnLoad = dontDestroyOnLoad;
        var resPath = cachePath.Replace(@"\\", "/");

        FileResLoader.AddFileAsset(resPath, fileAsset);
        AddResMapper(resPath, new ResMapper(cachePath, FileResLoader));
    }
    
    public void AddABAsset(string path)
    {
        var abAsset = new ABRefAsset(path);
        abAsset.LoadAssetBundle();
        
        var allAssetsName = abAsset.RefAssetBundle.GetAllAssetNames();
        foreach (var resPath in allAssetsName)
        {
            if(!resPath.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase))
                continue;
            
            AddResMapper(resPath, new ResMapper(resPath, ABResLoader));
        }
        
        ABResLoader.AddABAssetToFirst(abAsset);
    }

    public void AddResMapper(string path, ResMapper resMapper)
    {
        if (resMappers.ContainsKey(path))
        {
            Main.LogWarning($"覆盖Asset [{resMapper.resLoader.LoaderCode}]({path})");
        }
        else
        {
            Main.LogInfo($"添加Asset [{resMapper.resLoader.LoaderCode}]({path})");
        }
        resMappers[path] = resMapper;
    }

    public Sprite GetSpriteCache(Texture2D texture)
    {
        if (texture == null)
            return null;
        
        if(!spriteCache.TryGetValue(texture.GetHashCode(), out var sprite) || sprite == null)
        {
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            spriteCache.Add(texture.GetHashCode(),sprite);
        }
        return sprite;
    }

    #endregion

    public void ReleaseCache()
    {
        spriteCache.Clear();
        Resources.UnloadUnusedAssets();
    }
}