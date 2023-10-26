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
        var texture = Main.Res.LoadAsset<Texture2D>($"Assets/{path}.png");
        if (texture == null)
        {
            return true;
        }
        
        __result = Main.Res.GetSpriteCache(texture);
        return false;
    }
}