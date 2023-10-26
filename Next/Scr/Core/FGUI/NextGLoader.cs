using Cysharp.Threading.Tasks;
using FairyGUI;
using SkySwordKill.Next.Res;
using UnityEngine;

namespace SkySwordKill.Next.FGUI;

public class NextGLoader : GLoader
{
    public const string UI_FILE_PREFIX = "file://";
    
    protected override void LoadExternal()
    {
        var curUrl = this.url;
        if (curUrl.StartsWith(UI_FILE_PREFIX))
        {
            LoadFromFileAsync(curUrl).Forget();
        }
        else if(Main.Res.HaveAsset(curUrl))
        {
            LoadFromResourceManagerAsync(curUrl).Forget();
        }
        else
        {
            base.LoadExternal();
        }
    }

    private async UniTask LoadFromFileAsync(string curUrl)
    {
        var filePath = curUrl.Substring(UI_FILE_PREFIX.Length);
        var tagFile = new FileAsset(filePath);
        var asset = await tagFile.LoadAsync() as Texture2D;
        if (curUrl != url)
        {
            tagFile.Unload();
            return;
        }

        if (asset == null)
        {
            onExternalLoadFailed();
            return;
        }
        
        var nTex = asset.CreateTempNTexture();
        nTex.destroyMethod = DestroyMethod.Destroy;
        onExternalLoadSuccess(nTex);
    }

    private async UniTask LoadFromResourceManagerAsync(string curUrl)
    {
        var asset = await Main.Res.LoadAssetAsync<Texture2D>(curUrl);
        if (asset == null)
        {
            onExternalLoadFailed();
            return;
        }

        var nTex = asset.CreateTempNTexture();
        nTex.destroyMethod = DestroyMethod.None;
        onExternalLoadSuccess(nTex);
    }

    protected override void FreeExternal(NTexture nTexture)
    {
        nTexture.refCount--;
    }
}