using HarmonyLib;
using UnityEngine;

namespace SkySwordKill.Next.Patch
{
    /// <summary>
    /// 游戏资源加载通用Patch
    /// </summary>
    [HarmonyPatch(typeof(ModResources),"LoadTexture2D")]
    public class ModResourcesLoadTexturePatch
    {
        [HarmonyPrefix]
        public static bool LoadTexture2D(string path,ref Texture2D __result)
        {
            if (Main.Instance.resourcesManager.TryGetAsset($"Assets/Res/{path}.png", out var asset))
            {
                if (asset is Texture2D texture)
                {
                    __result = texture;
                    return false;
                }
            }
            return true;
        }
        
    }
}