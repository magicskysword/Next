using HarmonyLib;
using UnityEngine;

namespace SkySwordKill.Next.Patch;

/// <summary>
/// 游戏资源加载通用Patch
/// </summary>
[HarmonyPatch(typeof(ModResources),"LoadSprite")]
public class ModResourcesLoadSpritePatch
{
    [HarmonyPrefix]
    public static bool LoadSprite(string path,ref Sprite __result)
    {
        if (Main.Res.TryGetAsset($"Assets/Res/{path}.png", out var asset))
        {
            if (asset is Texture2D texture)
            {
                __result = Main.Res.GetSpriteCache(texture);
                return false;
            }
        }

        return true;
    }

}