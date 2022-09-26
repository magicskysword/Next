using System;
using System.Collections;
using System.IO;
using SkySwordKill.Next;
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
            Main.Res.StartCoroutine(LoadAsync());
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

    private IEnumerator LoadAsync()
    {
        if (isLoading || isDone)
            yield break;
        isLoading = true;
        Object loadAsset = null;
        var extension = Path.GetExtension(filePath);
        switch (extension)
        {
            case ".jpg":
            case ".png":
                using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(filePath))
                {
                    yield return webRequest.SendWebRequest();
                    if (webRequest.isDone)
                    {
                        var tex = DownloadHandlerTexture.GetContent(webRequest);
                        tex.hideFlags = HideFlags.HideAndDontSave;
                        loadAsset = tex;
                    }
                }

                break;
            case ".mp3":
                using (UnityWebRequest webRequest =
                       UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.UNKNOWN))
                {
                    yield return webRequest.SendWebRequest();
                    if (webRequest.isDone)
                    {
                        loadAsset = DownloadHandlerAudioClip.GetContent(webRequest);
                    }
                }

                break;
            case ".ab":
                using (UnityWebRequest webRequest = UnityWebRequestAssetBundle.GetAssetBundle(filePath))
                {
                    yield return webRequest.SendWebRequest();
                    if (webRequest.isDone)
                    {
                        var ab = DownloadHandlerAssetBundle.GetContent(webRequest);
                        loadAsset = ab;
                    }
                }

                break;
            case ".bytes":
                var bytes = File.ReadAllBytes(filePath);
                loadAsset = new BytesAsset(bytes);
                break;
        }

        if (!isDone && asset != null)
        {
            asset = loadAsset;
        }

        if (asset != null)
        {
            OnLoadedAsset?.Invoke(asset);
            isLoading = false;
            isDone = true;
        }
        else
        {
            Debug.LogWarning($"{filePath}加载失败");
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