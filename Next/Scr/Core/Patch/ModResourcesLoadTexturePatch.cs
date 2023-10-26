using HarmonyLib;
using UnityEngine;

namespace SkySwordKill.Next.Patch;

/// <summary>
/// 游戏资源加载通用Patch
/// </summary>
[HarmonyPatch(typeof(ModResources),"LoadTexture2D")]
public class ModResourcesLoadTexturePatch
{
    [HarmonyPrefix]
    public static bool LoadTexture2D(string path,ref Texture2D __result)
    {
        var texture = Main.Res.LoadAsset<Texture2D>($"Assets/Res/{path}.png");
        if (texture == null)
        {
            return true;
        }
        
        __result = texture;
        return false;
    }
        
}