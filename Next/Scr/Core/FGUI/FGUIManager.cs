using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next.Res;
using SkySwordKill.NextFGUI.NextCore;
using UnityEngine;

namespace SkySwordKill.Next.FGUI;

public class FGUIManager
{
    public const string PKG_NEXT_CORE = "NextCore";
        
    public AssetBundle FguiAB;
    public Dictionary<string, Shader> ShaderMap = new Dictionary<string, Shader>();
    public Dictionary<string, UIPackage> Packages = new Dictionary<string, UIPackage>();

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
            
        RegisterCursor("resizeH","Assets/Cursor/cursor_resize1.png");
        RegisterCursor("resizeV","Assets/Cursor/cursor_resize2.png");
        // 右下方向
        RegisterCursor("resizeRB","Assets/Cursor/cursor_resize3.png");
        // 左下方向
        RegisterCursor("resizeLB","Assets/Cursor/cursor_resize4.png");
        RegisterCursor("text","Assets/Cursor/cursor_text.png");
            
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
    }

    public void Reset()
    {
        Packages.Clear();
    }
        
    public void AddPackage(string pkgName)
    {
        if(Packages.ContainsKey(pkgName))
            return;

        var fguiPath = $"Assets/UIRes/{pkgName}";
        var fguiBytesPath = $"{fguiPath}_fui.bytes";
        if (Main.Res.TryGetAsset(fguiBytesPath, out var asset) && asset is BytesAsset bytesAsset)
        {
            var tagPackage = UIPackage.AddPackage(bytesAsset.Bytes , fguiPath, LoadResFunc);
            Packages.Add(pkgName, tagPackage);
            // 加载依赖
            foreach (var dependency in tagPackage.dependencies)
            {
                AddPackage(dependency["name"]);
            }
        }
        else
        {
            Main.LogWarning($"[FGUI]不存在UI包：{pkgName}");
        }
    }

    public GObject CreateUIObject(string pkgName, string resName)
    {
        AddPackage(pkgName);

        return UIPackage.CreateObject(pkgName, resName);
    }
        
    private void LoadFGUIAB()
    {
        FguiAB = AssetBundle.LoadFromFile($"{Main.PathAbDir.Value}/next_fairygui");
        foreach (var asset in FguiAB.LoadAllAssets())
        {
            if (asset is Shader shader)
            {
                ShaderMap.Add(shader.name, shader);
                Main.LogInfo($"[FGUI]加载Shader：{shader.name}");
            }
        }
    }
        
    private void RegisterCursor(string name,string path)
    {
        if (Main.Res.TryGetAsset(path, out var asset) && asset is Texture2D texture2D)
        {
            Stage.inst.RegisterCursor(name, texture2D, new Vector2(texture2D.width/2f,texture2D.height/2f));
        }
        else
        {
            Debug.LogWarning($"[FGUI]鼠标[{name}]资源不存在，路径：{path}");
        }
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
        string tagPath = name + extension;
        Main.LogDebug($"[FGUI]加载文件：<{type}> {tagPath}");
        if (Main.Res.HaveAsset(tagPath))
        {
            if (Main.Res.TryGetAsset(tagPath, out var asset))
            {
                return asset;
            }
        }
        Main.LogWarning($"[FGUI]不存在文件：<{type}> {tagPath}");
        return null;
    }
}