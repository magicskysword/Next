using HarmonyLib;
using UnityEngine;

namespace SkySwordKill.Next.Patch;

/// <summary>
/// 游戏资源加载Patch
/// </summary>
[HarmonyPatch(typeof(ResManager),"LoadSprite")]
public class ResManagerLoadSpritePatch
{
    [HarmonyPrefix]
    public static bool LoadSprite(ResManager __instance,string path,ref Sprite __result)
    {
        if (Main.Res.TryGetAsset($"Assets/{path}.png", out var asset))
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