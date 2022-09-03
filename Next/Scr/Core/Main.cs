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
using System.Collections;
using System.IO;
using Fungus;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.Next.Res;
using SkySwordKill.Next.FCanvas;

namespace SkySwordKill.Next
{
    [BepInPlugin("skyswordkill.plugin.Next", "Next", MOD_VERSION)]
    public partial class Main : BaseUnityPlugin
    {
        public const string MOD_VERSION = "0.6.1";
        
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
        
        public ConfigTarget<string> LanguageID;
        public ConfigTarget<bool> DebugMode;
        public ConfigTarget<bool> OpenInStart;
        public ConfigTarget<KeyCode> WinKeyCode;
        
        public NextLanguage NextLanguage;
        public NextModSetting NextModSetting;
        
        private ResourcesManager _resourcesManager;
        private LuaManager _luaManager;
        private FGUIManager _fguiManager;
        private FPatchManager _fPatchManager;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            I = this;
            
            InitDir();

            LanguageID = Config.CreateConfig("Main.Language", "Plugin Language", "",
                "");
            WinKeyCode = Config.CreateConfig("Main.OpenKeyCode", "Window HotKey", KeyCode.F4,
                "");
            OpenInStart = Config.CreateConfig("Main.OpenInStart", "Open Window In Game Start", true,
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

            DialogAnalysis.Init();

            // 初始化语言与配置
            // Init language and config
            NextLanguage.InitLanguage();
            LoadDefaultLanguage();
            NextModSetting = NextModSetting.LoadSetting();
            
            // 检查更新
            // Check Update
            Updater.CheckVersion();
            
            // 根据设置显示窗口
            // show window by config
            _isWinOpen = OpenInStart.Value;
            
            // ModManager 启动由 JsonDataPatch 进行引导
            // ModManager startup is booted by JsonDataPatch
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
            if (string.IsNullOrEmpty(LanguageID.Value) || !NextLanguage.languages.TryGetValue(LanguageID.Value, out var language))
            {
                // 选择一个语言作为默认语言
                // Choose Default Language
                SelectLanguage(NextLanguage.languages.Values.FirstOrDefault());
                isSelectedLanguage = true;
            }
            else
            {
                SelectLanguage(language);
            }
            
            LanguageID.SetName("Config.Main.LanguageID.Name".I18N());
            LanguageID.SetDesc("Config.Main.LanguageID.Desc".I18N());
            
            WinKeyCode.SetName("Config.Main.OpenKeyCode.Name".I18N());
            WinKeyCode.SetDesc("Config.Main.OpenKeyCode.Desc".I18N());
            
            OpenInStart.SetName("Config.Main.OpenInStart.Name".I18N());
            OpenInStart.SetDesc("Config.Main.OpenInStart.Desc".I18N());
            
            DebugMode.SetName("Config.Debug.Mode.Name".I18N());
            DebugMode.SetDesc("Config.Debug.Mode.Desc".I18N());
        }

        public void SelectLanguage(NextLanguage language)
        {
            NextLanguage = language;

            if (language == null)
                return;
            
            LanguageID.Value = language.FileName;
            LogInfo($"{"Misc.CurrentLanguage".I18N()} : {language.LanguageName}");
        }

        public void SaveModSetting()
        {
            NextModSetting.SaveSetting(NextModSetting);
        }

        public static Coroutine InvokeCoroutine(IEnumerator enumerator)
        {
            return I.StartCoroutine(enumerator);
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
}