using FairyGUI;
using SkySwordKill.Next.Res;
using UnityEngine;

namespace SkySwordKill.Next.FGUI
{
    public class NextGLoader : GLoader
    {
        public const string UI_FILE_PREFIX = "file://";

        protected override void LoadExternal()
        {
            var curUrl = this.url;
            if (curUrl.StartsWith(UI_FILE_PREFIX))
            {
                var filePath = curUrl.Substring(UI_FILE_PREFIX.Length);
                var tagFile = new FileAsset(filePath);
                var obj = tagFile.LoadAsset();
                if(obj != null && obj is Texture2D tex)
                {
                    var nTex = new NTexture(tex);
                    onExternalLoadSuccess(nTex);
                }
                else
                {
                    onExternalLoadFailed();
                }
            }
            else if(Main.Res.HaveAsset(curUrl))
            {
                Main.Res.TryGetAsset(curUrl, img =>
                {
                    if (curUrl != url)
                    {
                        return;
                    }

                    if (img != null && img is Texture2D tex)
                    {
                        var nTex = new NTexture(tex);
                        onExternalLoadSuccess(nTex);
                    }
                    else
                    {
                        onExternalLoadFailed();
                    }
                });
            }
            else
            {
                base.LoadExternal();
            }
        }

        protected override void FreeExternal(NTexture texture)
        {
            if(texture.nativeTexture != null && texture.nativeTexture.hideFlags == HideFlags.HideAndDontSave)
            {
                Object.Destroy(texture.nativeTexture);
            }
            texture.Dispose();
        }
    }
}