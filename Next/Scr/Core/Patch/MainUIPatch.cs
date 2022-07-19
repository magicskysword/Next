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
            var cameraGo = new GameObject("Camera");
            var camera = cameraGo.AddComponent<Camera>();
            camera.orthographic = true;
            camera.cullingMask = 1 << LayerMask.NameToLayer("UI");
            camera.depth = 0;
            var canvas = __instance.transform.parent.GetComponent<Canvas>();
            canvas.worldCamera = camera;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            var curPosition = __instance.transform.localPosition;
            curPosition.z = 0;
            __instance.transform.localPosition = curPosition;
        }
    }
}