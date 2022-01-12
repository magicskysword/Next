using HarmonyLib;
using UnityEngine;

namespace SkySwordKill.Next.Patch
{
    [HarmonyPatch(typeof(ResManager),"LoadSprite")]
    public class ResManagerLoadSpritePatch
    {
        [HarmonyPrefix]
        public static bool LoadSprite(ResManager __instance,string path,ref Sprite __result)
        {
            if (Main.Instance.resourcesManager.TryGetAsset($"Assets/{path}.png", out var asset))
            {
                if (asset is Texture2D texture)
                {
                    __result = Main.Instance.resourcesManager.GetSpriteCache(texture);
                    return false;
                }
            }

            return true;
        }
    }
}