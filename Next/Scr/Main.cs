using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.Mod;
using SkySwordKill.Next.Patch;
using SkySwordKill.Next.XiaoYeGUI;
using UnityEngine;

namespace SkySwordKill.Next
{
    [BepInPlugin("skyswordkill.plugin.Next", "Next", MOD_VERSION)]
    public partial class Main : BaseUnityPlugin
    {
        public const string MOD_VERSION = "0.3.1";
        
        public static Lazy<string> pathModsDir =
            new Lazy<string>(() => Utility.CombinePaths(
                BepInEx.Paths.PluginPath, "Next"));
        
        public static Lazy<string> pathLibraryDir =
            new Lazy<string>(() => Utility.CombinePaths(
                BepInEx.Paths.PluginPath, "NextLib"));
        
        public static Lazy<string> pathConfigDir =
            new Lazy<string>(() => Utility.CombinePaths(
                BepInEx.Paths.PluginPath, "NextConfig"));

        public static Lazy<string> pathBaseDataDir =
            new Lazy<string>(() => Utility.CombinePaths(
                pathModsDir.Value, "Base"));
        
        public static Lazy<string> pathLanguageDir =
            new Lazy<string>(() => Utility.CombinePaths(
                pathConfigDir.Value, "language"));
        
        public static Lazy<string> pathModSettingFile =
            new Lazy<string>(() => Utility.CombinePaths(
                pathConfigDir.Value, "modSetting.json"));

        public static Main Instance { get; private set; }
        public static int logIndent = 0;
        
        public ConfigTarget<string> languageID;
        public ConfigTarget<bool> debugMode;
        public ConfigTarget<bool> openInStart;
        public ConfigTarget<KeyCode> winKeyCode;
        
        public NextLanguage nextLanguage;
        public NextModSetting nextModSetting;

        public ResourcesManager resourcesManager;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            Instance = this;
            
            languageID = Config.CreateConfig("Main.Language", "Plugin Language", "",
                "");
            winKeyCode = Config.CreateConfig("Main.OpenKeyCode", "Window HotKey", KeyCode.F4,
                "");
            openInStart = Config.CreateConfig("Main.OpenInStart", "Open Window In Game Start", true,
                "");
            debugMode = Config.CreateConfig("Debug.Mode", "Debug Mode", false,
                "");
            
            resourcesManager = gameObject.AddComponent<ResourcesManager>();

            // 数据加载Patch
            // data patch
            Harmony.CreateAndPatchAll(typeof(JsonDataPatch));
            Harmony.CreateAndPatchAll(typeof(NpcJieSuanManagerPatch));

            // 资源相关Patch
            // resources patch
            Harmony.CreateAndPatchAll(typeof(SkillIconPatch));
            Harmony.CreateAndPatchAll(typeof(ItemUIPatch));

            Harmony.CreateAndPatchAll(typeof(BagActiveSkillGetIconSprite));
            Harmony.CreateAndPatchAll(typeof(BagPassiveSkillGetIconSprite));

            Harmony.CreateAndPatchAll(typeof(ResManagerLoadSpritePatch));
            Harmony.CreateAndPatchAll(typeof(ResManagerLoadTexturePatch));

            Harmony.CreateAndPatchAll(typeof(UIFightWeaponItemPatch));

            // 加载运行时脚本所需DLL
            // load runtime dll
            Assembly.LoadFrom(Utility.CombinePaths(pathLibraryDir.Value, "Microsoft.CSharp.dll"));

            DialogAnalysis.Init();

            // 初始化语言与配置
            // Init language and config
            NextLanguage.InitLanguage();
            LoadDefaultLanguage();
            nextModSetting = NextModSetting.LoadSetting();
            
            // 根据设置显示窗口
            // show window by config
            isWinOpen = openInStart.Value;
        }

        private void LoadDefaultLanguage()
        {
            if (string.IsNullOrEmpty(languageID.Value) || !NextLanguage.languages.TryGetValue(languageID.Value, out var language))
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
            
            languageID.SetName("Config.Main.LanguageID.Name".I18N());
            languageID.SetDesc("Config.Main.LanguageID.Desc".I18N());
            
            winKeyCode.SetName("Config.Main.OpenKeyCode.Name".I18N());
            winKeyCode.SetDesc("Config.Main.OpenKeyCode.Desc".I18N());
            
            openInStart.SetName("Config.Main.OpenInStart.Name".I18N());
            openInStart.SetDesc("Config.Main.OpenInStart.Desc".I18N());
            
            debugMode.SetName("Config.Debug.Mode.Name".I18N());
            debugMode.SetDesc("Config.Debug.Mode.Desc".I18N());
        }

        public void SelectLanguage(NextLanguage language)
        {
            nextLanguage = language;

            if (language == null)
                return;
            
            languageID.Value = language.FileName;
            LogInfo($"{"Misc.CurrentLanguage".I18N()} : {language.LanguageName}");
        }

        public void SaveModSetting()
        {
            NextModSetting.SaveSetting(nextModSetting);
        }

        public static Coroutine InvokeCoroutine(IEnumerator enumerator)
        {
            return Instance.StartCoroutine(enumerator);
        }

        public static void LogInfo(object obj)
        {
            Instance.Logger.LogInfo($"{GetIndent()}{obj}");
        }

        public static void LogWarning(object obj)
        {
            Instance.Logger.LogWarning($"{GetIndent()}{obj}");
        }

        public static void LogError(object obj)
        {
            Instance.Logger.LogError($"{GetIndent()}{obj}");
        }
        
        public static void LogDebug(object obj)
        {
            if (Instance.debugMode.Value)
            {
                Instance.Logger.LogInfo($"[Debug]{obj}");
            }
        }

        private static string GetIndent()
        {
            if (logIndent <= 0)
                return string.Empty;
            var sb = new StringBuilder(logIndent * 4);
            for (int i = 0; i < logIndent * 4; i++)
            {
                sb.Append(' ');
            }

            return sb.ToString();
        }
    }
    

    

    

    

    

    

    

    

    
}