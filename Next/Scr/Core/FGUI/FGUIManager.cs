using System;
using System.Collections.Generic;
using FairyGUI;
using Next.Scr.Core.FGUI;
using SkySwordKill.Next.Res;
using SkySwordKill.NextFGUI.NextCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SkySwordKill.Next.FGUI;

public class FGUIManager : MonoBehaviour
{
    public const string PKG_NEXT_CORE = "NextCore";
        
    public AssetBundle FguiAB;
    public Dictionary<string, Shader> ShaderMap = new Dictionary<string, Shader>();
    public Dictionary<string, UIPackage> Packages = new Dictionary<string, UIPackage>();
    public List<FGUIScenePanelBase> ScenePanels = new List<FGUIScenePanelBase>();
    public List<FGUIWindowBase> Windows = new List<FGUIWindowBase>();

    public const string MOUSE_RESIZE_H = "resizeH";
    public const string MOUSE_RESIZE_V = "resizeV";
    public const string MOUSE_RESIZE_BR = "resizeBR";
    public const string MOUSE_RESIZE_BL = "resizeBL";

    public const string MOUSE_TEXT = "text";
    public const string MOUSE_HAND = "hand";

    private Canvas _fguiCanvas;
    private RenderTexture _renderTexture;
    
    int lastScreenWidth = 0;
    int lastScreenHeight = 0;

    public void Init()
    {
        LoadFGUIAB();
        FontManager.RegisterFont(new DynamicFont("Alibaba-PuHuiTi-Medium",
            FguiAB.LoadAsset<Font>("Alibaba-PuHuiTi-Medium")));

        UIConfig.defaultFont = "Alibaba-PuHuiTi-Medium";
        UIConfig.horizontalScrollBar = "ui://NextCore/ScrollBarH";
        UIConfig.verticalScrollBar = "ui://NextCore/ScrollBarV";
        UIConfig.popupMenu = "ui://NextCore/PopupMenu";
        UIConfig.popupMenu_seperator = "ui://NextCore/PopupMenu_separator";
        UIConfig.tooltipsWin = "ui://NextCore/TooltipsWin";
        UIConfig.loaderErrorSign = "ui://NextCore/icon_false";
            
        UIObjectFactory.SetLoaderExtension(() => new NextGLoader());

        ShaderConfig.Get = GetShaderInAB;
            
        RegisterCursor(MOUSE_RESIZE_H,"Assets/Cursor/cursor_resize1.png");
        RegisterCursor(MOUSE_RESIZE_V,"Assets/Cursor/cursor_resize2.png");
        // 右下方向
        RegisterCursor(MOUSE_RESIZE_BR,"Assets/Cursor/cursor_resize3.png");
        // 左下方向
        RegisterCursor(MOUSE_RESIZE_BL,"Assets/Cursor/cursor_resize4.png");
        RegisterCursor(MOUSE_TEXT,"Assets/Cursor/cursor_text.png");
        RegisterCursor(MOUSE_HAND,"Assets/Cursor/cursor_hand.png");
            
        AddPackage(PKG_NEXT_CORE);
            
        NextCoreBinder.BindAll();

        GRoot.inst.SetContentScaleFactor(1);
            
        // 将StageCamera的Layer设置为17层
        StageCamera.CheckMainCamera();
        GameObject.DontDestroyOnLoad(StageCamera.main.gameObject);
        StageCamera.main.cullingMask = 1 << 17;
            
        // 将Stage和所有子物体的Layer设置为17层
        GameObject stage = Stage.inst.gameObject;
        stage.layer = 17;
        foreach (Transform child in stage.transform)
        {
            child.gameObject.layer = 17;
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // 创建一个Canvas，用于显示FGUI
        var obj = new GameObject("FGUICanvas");
        DontDestroyOnLoad(obj);
        _fguiCanvas = obj.AddComponent<Canvas>();
        _fguiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _fguiCanvas.sortingOrder = 900;

        // 将StageCamera的内容通过RenderTexure渲染到FGUI的Canvas上
        var camera = StageCamera.main.GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        _renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        camera.targetTexture = _renderTexture;
        var imageObj = new GameObject("FGUIRawImage");
        var imageRect = imageObj.AddComponent<RectTransform>();
        imageObj.transform.SetParent(_fguiCanvas.transform);
        // 将imageRect设为全屏
        imageRect.anchorMin = Vector2.zero;
        imageRect.anchorMax = Vector2.one;
        imageRect.offsetMin = Vector2.zero;
        imageRect.offsetMax = Vector2.zero;
        var image = imageObj.AddComponent<RawImage>();
        image.texture = _renderTexture;
        
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
    }
    
    void Update()
    {
        if (lastScreenWidth != Screen.width || lastScreenHeight != Screen.height)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            OnScreenSizeChanged();
        }
    }

    private void OnScreenSizeChanged()
    {
        _renderTexture.Release();
        _renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        StageCamera.main.GetComponent<Camera>().targetTexture = _renderTexture;
        var image = _fguiCanvas.GetComponentInChildren<RawImage>();
        image.texture = _renderTexture;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        ResetCamera();

        // 清除场景UI
        foreach (var sceneComponent in ScenePanels.ToArray())
        {
            if(!sceneComponent.DontDestroyOnLoad)
                sceneComponent.Hide();
        }
    }
    
    public void RemoveAllWindow(Func<FGUIWindowBase, bool> checker = null)
    {
        foreach (var window in Windows.ToArray())
        {
            try
            {
                if(checker == null || checker(window))
                    window.Hide();
            }
            catch (Exception e)
            {
                Main.LogError(e);
            }
        }
    }

    public void RemoveAllWindowForce()
    {
        foreach (var window in Windows.ToArray())
        {
            try
            {
                window.HideForce();
            }
            catch (Exception e)
            {
                Main.LogError(e);
            }
        }
    }
    
    public void ResetCamera()
    {
        StageCamera.main.cullingMask = 1 << 17;
        var cameras = GameObject.FindObjectsOfType<Camera>();
        foreach (var camera in cameras)
        {
            // 为场景所有相机设置遮罩
            if(camera != StageCamera.main.GetComponent<Camera>())
            {
                // 将主摄像机的Layer设置为排除17层
                camera.cullingMask &= ~(1 << 17);
            }
        }
    }
 
    public void Reset()
    {
        Packages.Clear();
    }
        
    public void AddPackage(string pkgName)
    {
        // 防止重复加载
        if(Packages.ContainsKey(pkgName))
            return;

        var fguiPath = $"Assets/UIRes/{pkgName}";
        var fguiBytesPath = $"{fguiPath}_fui.bytes";

        var bytes = Main.Res.LoadBytes(fguiBytesPath);
        if(bytes == null)
        {
            Main.LogError($"[FGUI]不存在UI包：{pkgName}");
            return;
        }
        
        var tagPackage = UIPackage.AddPackage(bytes, fguiPath, LoadResFunc);
        Packages.Add(pkgName, tagPackage);
        // 加载依赖
        foreach (var dependency in tagPackage.dependencies)
        {
            AddPackage(dependency["name"]);
        }
    }

    public GObject CreateUIObject(string pkgName, string resName)
    {
        AddPackage(pkgName);

        return UIPackage.CreateObject(pkgName, resName);
    }
        
    private void LoadFGUIAB()
    {
        FguiAB = AssetBundle.LoadFromFile($"{Main.PathInnerAbDir.Value}/next_fairygui");
        foreach (var asset in FguiAB.LoadAllAssets())
        {
            if (asset is Shader shader)
            {
                ShaderMap.Add(shader.name, shader);
                Main.LogInfo($"[FGUI]加载Shader：{shader.name}");
            }
        }
    }
        
    private void RegisterCursor(string cursorName,string path, float hotspotX = 0.5f, float hotspotY = 0.5f)
    {
        var texture2D = Main.Res.LoadAsset<Texture2D>(path);
        if (texture2D == null)
        {
            Debug.LogWarning($"[FGUI]鼠标[{cursorName}]资源不存在，路径：{path}");
            return;
        }
        
        var hotspot = new Vector2(texture2D.width * hotspotX, texture2D.height * hotspotY);
        
        Stage.inst.RegisterCursor(cursorName, texture2D, hotspot);
    }

    private Shader GetShaderInAB(string name)
    {
        if (ShaderMap.TryGetValue(name, out var shader))
        {
            return shader;
        }

        return null;
    }
        
    private static object LoadResFunc(string name, string extension, System.Type type, out DestroyMethod destroyMethod)
    {
        destroyMethod = DestroyMethod.None;
        string tagPath = $"{name}{extension}".Replace("\\", "/");
        Main.LogDebug($"[FGUI]加载文件：<{type}> {tagPath}");
        var asset = Main.Res.LoadAsset(tagPath);
        
        if (asset == null)
        {
            Main.LogWarning($"[FGUI]不存在文件：<{type}> {tagPath}");
        }

        return asset;
    }

    public void RegisterScenePanel(FGUIScenePanelBase scenePanel)
    {
        ScenePanels.Add(scenePanel);
    }

    public void RemoveScenePanel(FGUIScenePanelBase scenePanel)
    {
        ScenePanels.Remove(scenePanel);
    }

    public void RegisterWindow(FGUIWindowBase fguiWindowBase)
    {
        Windows.Add(fguiWindowBase);
    }

    public void UnRegisterWindow(FGUIWindowBase fguiWindowBase)
    {
        Windows.Remove(fguiWindowBase);
    }

    public void AutoCloseUIOnLoadScene()
    {
        RemoveAllWindow(window => window.AutoCloseInSceneChange);
    }

    public void AutoCloseUIOnSaveQuit()
    {
        RemoveAllWindow(window => window.AutoCloseInQuitSave);
    }
}