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
        var texture = Main.Res.LoadAsset<Texture2D>($"Assets/{path}.png");
        if (texture == null)
        {
            return true;
        }
        
        __result = texture;
        return false;
    }
}