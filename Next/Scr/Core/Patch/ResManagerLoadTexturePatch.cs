using HarmonyLib;
using UnityEngine;

namespace SkySwordKill.Next.Patch;

/// <summary>
/// 游戏资源加载Patch
/// </summary>
[HarmonyPatch(typeof(ResManager),"LoadTexture2D")]
public class ResManagerLoadTexturePatch
{
    [HarmonyPrefix]
    public static bool LoadTexture2D(ResManager __instance,string path,ref Texture2D __result)
    {
        if (Main.Res.TryGetAsset($"Assets/{path}.png", out var asset))
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