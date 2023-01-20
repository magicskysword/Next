using System;
using UnityEngine;
using SkySwordKill.Next.Lua;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI;
using HarmonyLib;
using BepInEx;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.Next.Res;
using SkySwordKill.Next.FCanvas;
using SkySwordKill.Next.I18N;
using SkySwordKill.Next.Mod;
using UnityEngine.SceneManagement;

namespace SkySwordKill.Next;

[BepInPlugin("skyswordkill.plugin.Next", "Next", MOD_VERSION)]
public partial class Main : BaseUnityPlugin
{
    public const string MOD_VERSION = "0.7.5";
        
    public static Lazy<string> PathLocalModsDir;
    public static Lazy<string> PathLibraryDir;
    public static Lazy<string> PathConfigDir;
    public static Lazy<string> PathExportOutputDir;
    public static Lazy<string> PathBaseDataDir;
    public static Lazy<string> PathLuaLibDir;
    public static Lazy<string> PathLanguageDir;
    public static Lazy<string> PathModSettingFile;
    public static Lazy<string> PathInnerAssetDir;
    public static Lazy<string> PathBaseFungusDataDir;
    public static Lazy<string> PathAbDir;
        
    public static Main Instance => I;
    public static Main I { get; private set; }
    public static ResourcesManager Res => I._resourcesManager;
    public static LuaManager Lua => I._luaManager;
    public static FGUIManager FGUI => I._fguiManager;
    public static FPatchManager FPatch => I._fPatchManager;
        
    public static int LogIndent = 0;
        
    public ConfigTarget<string> LanguageDir;
    public ConfigTarget<bool> DebugMode;
    public ConfigTarget<KeyCode> WinKeyCode;

    public NextLanguage CurrentLanguage { get; private set; }
    public NextModSetting NextModSetting;
        
    private ResourcesManager _resourcesManager;
    private LuaManager _luaManager;
    private FGUIManager _fguiManager;
    private FPatchManager _fPatchManager;
    
    private Dictionary<string, List<Action<Scene>>> _sceneLoadActions = new Dictionary<string, List<Action<Scene>>>();

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        AfterInit();
    }

    private void Init()
    {
        I = this;
            
        InitDir();

        LanguageDir = Config.CreateConfig("Main.Language", "Plugin Language", "",
            "");
        WinKeyCode = Config.CreateConfig("Main.OpenKeyCode", "Window HotKey", KeyCode.F4,
            "");
        DebugMode = Config.CreateConfig("Debug.Mode", "Debug Mode", false,
            "");
            
        _resourcesManager = gameObject.AddComponent<ResourcesManager>();
        _resourcesManager.Init();
            
        _luaManager = new LuaManager();
        _luaManager.Init();

        _fguiManager = new FGUIManager();
        _fguiManager.Init();

        _fPatchManager = new FPatchManager();
        _fPatchManager.Init();
            
        new Harmony("skyswordkill.plugin.Next").PatchAll();

        // 加载运行时脚本所需DLL
        // load runtime dll
        Assembly.LoadFrom(Utility.CombinePaths(PathLibraryDir.Value, "Microsoft.CSharp.dll"));
        // Assembly.LoadFrom(Utility.CombinePaths(PathLibraryDir.Value, "System.Windows.Forms.dll"));
        // Assembly.LoadFrom(Utility.CombinePaths(PathLibraryDir.Value, "Ookii.Dialogs.dll"));
        
        // 初始化语言与配置
        // Init language and config
        NextLanguage.InitLanguage();
        LoadDefaultLanguage();
        LoadModSetting();

        SceneManager.sceneLoaded += OnSceneLoaded;
        
        RegisterSceneLoadEvent("MainMenu", scene =>
        {
            // 检查Mod重载情况
            ModManager.CheckModLoadState();
        });

        // ModManager 启动由 JsonDataPatch 进行引导
        // ModManager startup is booted by JsonDataPatch
    }

    private void AfterInit()
    {
        DialogAnalysis.Init();
        
        ModSettingDefinitionListConverter.Init();
        
        // 检查更新
        // Check Update
        Updater.CheckVersion();
        
        _isWinOpen = false;
    }

    private void InitDir()
    {
        var dllPath = Directory.GetParent(typeof(Main).Assembly.Location).FullName;

        PathLocalModsDir =
            new Lazy<string>(() => BepInEx.Paths.GameRootPath + @"\本地Mod测试");
        PathLibraryDir =
            new Lazy<string>(() => Utility.CombinePaths(
                dllPath,
                "NextLib"));
        PathConfigDir =
            new Lazy<string>(() => Utility.CombinePaths(
                dllPath,
                "NextConfig"));
        PathExportOutputDir = 
            new Lazy<string>(() => Utility.CombinePaths(
                dllPath,
                "../OutPut"));
        PathBaseDataDir =
            new Lazy<string>(() => Utility.CombinePaths(
                PathExportOutputDir.Value,
                "Data"));
        PathBaseFungusDataDir =
            new Lazy<string>(() => Utility.CombinePaths(
                PathExportOutputDir.Value, 
                "Fungus"));
        PathLuaLibDir =
            new Lazy<string>(() => Utility.CombinePaths(
                PathLibraryDir.Value, "Lua"));
        PathAbDir =
            new Lazy<string>(() => Utility.CombinePaths(
                PathLibraryDir.Value, "AB"));
        PathLanguageDir =
            new Lazy<string>(() => Utility.CombinePaths(
                PathConfigDir.Value, "language"));
        PathModSettingFile =
            new Lazy<string>(() => Utility.CombinePaths(
                BepInEx.Paths.GameRootPath, "nextModSetting.json"));
        PathInnerAssetDir =
            new Lazy<string>(() => Utility.CombinePaths(
                dllPath, "NextAssets"));
    }

    private void LoadDefaultLanguage()
    {
        if (string.IsNullOrEmpty(LanguageDir.Value) || !NextLanguage.TryGetLanguageByDir(LanguageDir.Value, out var language))
        {
            // 选择一个语言作为默认语言
            // Choose Default Language
            SelectLanguage(NextLanguage.Languages.FirstOrDefault());
            isSelectedLanguage = true;
        }
        else
        {
            SelectLanguage(language);
        }
            
        LanguageDir.SetName("Config.Main.LanguageID.Name".I18N());
        LanguageDir.SetDesc("Config.Main.LanguageID.Desc".I18N());
            
        WinKeyCode.SetName("Config.Main.OpenKeyCode.Name".I18N());
        WinKeyCode.SetDesc("Config.Main.OpenKeyCode.Desc".I18N());

        DebugMode.SetName("Config.Debug.Mode.Name".I18N());
        DebugMode.SetDesc("Config.Debug.Mode.Desc".I18N());
    }

    public void SelectLanguage(NextLanguage language)
    {
        CurrentLanguage = language;

        if (language == null)
            return;
            
        LanguageDir.Value = language.LanguageDir;
        LogInfo($"{"Misc.CurrentLanguage".I18N()} : {language.Config.LanguageName}");
    }

    internal void LoadModSetting()
    {
        NextModSetting = NextModSetting.LoadSetting();
    }
    
    internal void SaveModSetting()
    {
        NextModSetting.SaveSetting(NextModSetting);
    }

    private void OnSceneLoaded(Scene loadScene, LoadSceneMode loadSceneMode)
    {
        if(_sceneLoadActions.TryGetValue(loadScene.name, out var actions))
        {
            foreach (var action in actions)
            {
                try
                {
                    action(loadScene);
                }
                catch (Exception e)
                {
                    LogError(e);
                }
            }
        }
    }

    public void RegisterSceneLoadEvent(string sceneName, Action<Scene> action)
    {
        if (!_sceneLoadActions.TryGetValue(sceneName, out var list))
        {
            _sceneLoadActions.Add(sceneName, list = new List<Action<Scene>>());
        }
        list.Add(action);
    }
    
    public static void LogLua(object obj)
    {
        I.Logger.LogInfo($"Lua:\t{obj}");
    }
        
    public static void LogInfo(object obj)
    {
        I.Logger.LogInfo($"{GetIndent()}{obj}");
    }

    public static void LogWarning(object obj)
    {
        I.Logger.LogWarning($"{GetIndent()}{obj}");
    }

    public static void LogError(object obj)
    {
        I.Logger.LogError($"{GetIndent()}{obj}");
    }
        
    public static void LogDebug(object obj)
    {
        if (I.DebugMode.Value)
        {
            I.Logger.LogInfo($"[Debug]{obj}");
        }
    }

    private static string GetIndent()
    {
        if (LogIndent <= 0)
            return string.Empty;
        var sb = new StringBuilder(LogIndent * 4);
        for (int i = 0; i < LogIndent * 4; i++)
        {
            sb.Append(' ');
        }

        return sb.ToString();
    }
}

