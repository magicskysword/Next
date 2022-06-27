using FairyGUI;
using HarmonyLib;
using UnityEngine;

namespace SkySwordKill.Next.Patch
{
    [HarmonyPatch(typeof(MainUIMag),"Awake")]
    public class MainUIPatch
    {
        [HarmonyPostfix]
        public static void AfterAwake(MainUIMag __instance)
        {
            StageCamera.CheckMainCamera();
            var camera = StageCamera.main;
            var canvas = __instance.transform.parent.GetComponent<Canvas>();
            canvas.worldCamera = camera;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            var curPosition = __instance.transform.localPosition;
            curPosition.z = -10800;
            __instance.transform.localPosition = curPosition;
        }
    }
}