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
        var texture = Main.Res.LoadAsset<Texture2D>($"Assets/Res/{path}.png");
        if (texture == null)
        {
            return true;
        }
        
        __result = Main.Res.GetSpriteCache(texture);
        return false;
    }

}