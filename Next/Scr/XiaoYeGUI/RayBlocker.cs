/*
参考 https://github.com/ginko1/VRoidXYTool/blob/main/VRoidXYTool/RayBlocker.cs
*/

using UnityEngine;
using UnityEngine.UI;

namespace SkySwordKill.Next.XiaoYeGUI
{
    public class RayBlocker
    {
        private RectTransform rt;
        private GameObject canvasObj;

        private RayBlocker()
        {
            
        }

        public static RayBlocker CreateRayBlock()
        {
            var rayBlocker = new RayBlocker();
            
            rayBlocker.canvasObj = new GameObject("NextBlockerCanvas");
            rayBlocker.canvasObj.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            rayBlocker.canvasObj.AddComponent<GraphicRaycaster>();
            var gameObject = new GameObject("RayBlocker");
            rayBlocker.rt = gameObject.AddComponent<RectTransform>();
            rayBlocker.rt.SetParent(rayBlocker.canvasObj.transform);
            rayBlocker.rt.pivot = new Vector2(0, 1);
            Image rbImage = gameObject.AddComponent<Image>();
            rbImage.color = Color.clear;
            rbImage.raycastTarget = true;
            rayBlocker.CloseBlocker();
            
            GameObject.DontDestroyOnLoad(rayBlocker.canvasObj);

            return rayBlocker;
        }

        public void SetSize(Rect rect)
        {
            rt.sizeDelta = rect.size;
            rt.position = new Vector3(rect.position.x,Screen.height - rect.position.y);
        }

        public void OpenBlocker()
        {
            canvasObj.SetActive(true);
        }

        public void CloseBlocker()
        {
            canvasObj.SetActive(false);
        }
    }
}