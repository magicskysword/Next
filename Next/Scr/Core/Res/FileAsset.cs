using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace SkySwordKill.Next.Res;

public class FileAsset
{
    #region 字段

    /// <summary>
    /// 资源
    /// </summary>
    private Object asset;
    /// <summary>
    /// 文件路径
    /// </summary>
    private string filePath;
    /// <summary>
    /// 已经加载完成
    /// </summary>
    private bool isDone = false;
    /// <summary>
    /// 正在异步加载
    /// </summary>
    private bool isAsyncLoading = false;
    
    public bool DontDestroyOnLoad { get; set; } = false;

    public FileAsset(string path)
    {
        filePath = path;
    }

    #endregion

    #region 属性

    public Object Asset => asset;
    public bool IsDone => isDone;
    public bool IsAsyncLoading => isAsyncLoading;
    public string FileRawPath => filePath;

    #endregion

    #region 回调方法

    #endregion

    #region 公共方法

    public async UniTask<Object> LoadAsync()
    {
        if (isDone)
        {
            return asset;
        }
        
        await LoadAsyncInner();

        return asset;
    }

    public Object Load()
    {
        if (isDone)
        {
            return asset;
        }

        LoadInner();
        return asset;
    }

    public void Unload()
    {
        if (DontDestroyOnLoad)
            return;
        
        if (asset != null)
        {
            Object.Destroy(asset);
            isDone = false;
        }
    }
    
    #endregion

    #region 私有方法

    /// <summary>
    /// 异步加载
    /// </summary>
    private async UniTask LoadAsyncInner()
    {
        if (isDone)
            return;

        if (isAsyncLoading)
        {
            // 自旋等待加载完成
            while (isAsyncLoading)
            {
                await UniTask.Yield();
            }
            return;
        }

        isAsyncLoading = true;

        Object loadAsset = null;
        var extension = Path.GetExtension(filePath).ToLower();
        try
        {
            switch (extension)
            {
                case ".jpg":
                case ".png":
                    using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(filePath))
                    {
                        await webRequest.SendWebRequest();
                        var tex = DownloadHandlerTexture.GetContent(webRequest);
                        tex.hideFlags = HideFlags.HideAndDontSave;
                        loadAsset = tex;
                    }

                    break;
                case ".mp3":
                    using (UnityWebRequest webRequest =
                           UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.UNKNOWN))
                    {
                        await webRequest.SendWebRequest();
                        loadAsset = DownloadHandlerAudioClip.GetContent(webRequest);
                    }

                    break;
                case ".bytes":
                    var bytes = File.ReadAllBytes(filePath);
                    loadAsset = new BytesAsset(bytes);
                    break;
            }
        }
        catch (Exception e)
        {
            Main.LogError(e);
            Main.LogError($"FileAsset:{filePath} 异步加载失败");
            isAsyncLoading = false;
            return;
        }

        if (isDone)
        {
            return;
        }

        if (loadAsset != null)
        {
            asset = loadAsset;
        }
        isAsyncLoading = false;
        isDone = true;
    }

    private void LoadInner()
    {
        try
        {
            Object loadAsset = null;
            var extension = Path.GetExtension(filePath).ToLower();
            switch (extension)
            {
                case ".jpg":
                case ".png":
                {
                    var bytes = File.ReadAllBytes(filePath);
                    var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    texture.hideFlags = HideFlags.HideAndDontSave;
                    texture.LoadImage(bytes);
                    loadAsset = texture;
                }
                    break;
                case ".mp3":
                    using (UnityWebRequest webRequest =
                           UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.UNKNOWN))
                    {
                        webRequest.SendWebRequest();
                        while (!webRequest.isDone)
                        {
                            
                        }
                        loadAsset = DownloadHandlerAudioClip.GetContent(webRequest);
                        break;
                    }
                case ".bytes":
                {
                    var bytes = File.ReadAllBytes(filePath);
                    loadAsset = new BytesAsset(bytes);
                    break;
                }
                default:
                {
                    Main.LogError($"FileAsset:{filePath} 不属于可加载的文件");
                    break;
                }
            }
            if (loadAsset != null)
            {
                asset = loadAsset;
                isDone = true;
            }
        }
        catch (Exception e)
        {
            Main.LogError(e);
            Main.LogError($"FileAsset:{filePath} 同步加载失败");
            return;
        }
        
        
    }

    #endregion
}