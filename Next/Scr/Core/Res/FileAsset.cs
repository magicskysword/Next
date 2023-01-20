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

    public Object asset;
    public event Action<Object> OnLoadedAsset;

    private string filePath;
    private bool isDone = false;
    private bool isLoading = false;

    public FileAsset(string path)
    {
        filePath = path;
    }

    #endregion

    #region 属性

    public bool IsDone => isDone;
    public bool IsLoading => isLoading;
    public string FileRawPath => filePath;

    #endregion

    #region 回调方法

    #endregion

    #region 公共方法

    public void LoadAssetAsync(Action<Object> callback)
    {
        if (!isDone)
        {
            OnLoadedAsset += callback;
            LoadAsync().Forget();
        }
        else
        {
            callback?.Invoke(asset);
        }
    }

    public Object LoadAsset()
    {
        if (isDone)
        {
            return asset;
        }

        try
        {
            Load();
        }
        catch (Exception e)
        {
            Main.LogError(e);
            return null;
        }
        return asset;
    }

    #endregion

    #region 私有方法

    private async UniTaskVoid LoadAsync()
    {
        if (isLoading || isDone)
            return;
        isLoading = true;
        Object loadAsset = null;
        var extension = Path.GetExtension(filePath);
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
                case ".ab":
                    using (UnityWebRequest webRequest = UnityWebRequestAssetBundle.GetAssetBundle(filePath))
                    {
                        await webRequest.SendWebRequest();
                        var ab = DownloadHandlerAssetBundle.GetContent(webRequest);
                        loadAsset = ab;
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
            Debug.LogWarning($"{filePath}加载失败");
            isLoading = false;
            isDone = false;
            return;
        }

        if (!isDone && loadAsset != null)
        {
            asset = loadAsset;
        }

        if (asset != null)
        {
            asset.name = filePath;
            OnLoadedAsset?.Invoke(asset);
            isLoading = false;
            isDone = true;
        }
        else
        {
            Main.LogWarning($"{filePath}加载失败");
            isLoading = false;
            isDone = false;
        }
    }

    private void Load()
    {
        Object loadAsset = null;
        var extension = Path.GetExtension(filePath);
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
            case ".ab":
            {
                asset = AssetBundle.LoadFromFile(filePath);
                break;
            }
            case ".bytes":
            {
                var bytes = File.ReadAllBytes(filePath);
                loadAsset = new BytesAsset(bytes);
                break;
            }
        }

        if (loadAsset != null)
        {
            asset = loadAsset;
            isDone = true;
        }
    }

    #endregion
}