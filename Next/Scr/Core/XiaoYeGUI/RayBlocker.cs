/*
参考 https://github.com/ginko1/VRoidXYTool/blob/main/VRoidXYTool/RayBlocker.cs
*/

using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace SkySwordKill.Next.XiaoYeGUI;

public class RayBlocker
{
    private class GUIBlocker : MonoBehaviour
    {
        private GUIStyle _style;
        public Rect rect;

        private void Awake()
        {
            // _rect = GetComponent<RectTransform>();
            _style = new GUIStyle();
        }

        private void OnGUI()
        {
                
            GUI.Button(rect, "", _style);
        }
    }
        
    private RectTransform _rt;
    private GameObject _canvasObj;
    private GUIBlocker _guiBlocker;

    private RayBlocker()
    {
            
    }

    public static RayBlocker CreateRayBlock(string name = "NextBlockerCanvas")
    {
        var rayBlocker = new RayBlocker();
            
        rayBlocker._canvasObj = new GameObject(name);
            
        var canvas = rayBlocker._canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 800;
        rayBlocker._canvasObj.AddComponent<GraphicRaycaster>();
            
        var gameObject = new GameObject("RayBlocker");
        
        rayBlocker._rt = gameObject.AddComponent<RectTransform>();
        rayBlocker._rt.SetParent(rayBlocker._canvasObj.transform);
        rayBlocker._rt.pivot = new Vector2(0, 1);
        Image rbImage = gameObject.AddComponent<Image>();
        rbImage.color = Color.clear;
        rbImage.raycastTarget = true;
        rayBlocker.CloseBlocker();

        rayBlocker._guiBlocker = gameObject.AddComponent<GUIBlocker>();
            
        GameObject.DontDestroyOnLoad(rayBlocker._canvasObj);

        return rayBlocker;
    }

    public void SetSize(Rect rect)
    {
        _rt.sizeDelta = rect.size;
        _rt.position = new Vector3(rect.position.x,Screen.height - rect.position.y);
        _guiBlocker.rect = new Rect(rect);
    }

    public void OpenBlocker()
    {
        _canvasObj.SetActive(true);
    }

    public void CloseBlocker()
    {
        _canvasObj.SetActive(false);
    }

    public void DestroySelf()
    {
        Object.Destroy(_canvasObj);
    }
}