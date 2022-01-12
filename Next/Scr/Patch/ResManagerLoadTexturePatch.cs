﻿using HarmonyLib;
using UnityEngine;

namespace SkySwordKill.Next.Patch
{
    [HarmonyPatch(typeof(ResManager),"LoadTexture2D")]
    public class ResManagerLoadTexturePatch
    {
        [HarmonyPrefix]
        public static bool LoadTexture2D(ResManager __instance,string path,ref Texture2D __result)
        {
            if (Main.Instance.resourcesManager.TryGetAsset($"Assets/{path}.png", out var asset))
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