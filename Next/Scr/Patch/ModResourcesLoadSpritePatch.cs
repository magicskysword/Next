using HarmonyLib;
using UnityEngine;

namespace SkySwordKill.Next.Patch
{
    [HarmonyPatch(typeof(ModResources),"LoadSprite")]
    public class ModResourcesLoadSpritePatch
    {
        [HarmonyPrefix]
        public static bool LoadSprite(string path,ref Sprite __result)
        {
            if (Main.Instance.resourcesManager.TryGetAsset($"Assets/Res/{path}.png", out var asset))
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