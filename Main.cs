using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using YSGame.TuJian;

namespace SkySwordKill.Next
{
    [BepInPlugin("skyswordkill.plugin.Next", "Next", "0.2.1")]
    public partial class Main : BaseUnityPlugin
    {
        public static Main Instance;
        
        public ConfigEntry<string> gameVersion;
        public ConfigEntry<bool> debugMode;
        public ConfigEntry<bool> openInStart;
        public ConfigEntry<KeyCode> debugKeyCode;

        private void Awake()
        {
            Init();
        }

        public void Patch()
        {
            if (gameVersion.Value != Application.version)
            {
                DataPatcher.GenerateBaseData();
            }

            gameVersion.Value = Application.version;
            DataPatcher.LoadAllMod();
        }

        private void Init()
        {
            Instance = this;
            gameVersion = Config.Bind("General.GameVersion", "游戏版本", "",
                "游戏当前的版本，版本与配置不一致时会重新生成Base文件夹。");
            debugKeyCode = Config.Bind("Debug.OpenKeyCode", "调试窗口快捷键", KeyCode.F4,
                "Next插件调试窗口，用于查看当前加载mod以及进行Mod调试（需打开调试模式）。");
            debugMode = Config.Bind("Debug.Mode", "调试模式开关", false,
                "Next插件调试模式开关，打开后才能在调试窗口里看到更多功能。");
            openInStart = Config.Bind("Debug.OpenInStart", "游戏启动时弹出", true,
                "是否在游戏启动时弹出调试窗口。");
            DialogAnalysis.Init();
            Harmony.CreateAndPatchAll(typeof(JsonDataPatch));
            Assembly.LoadFrom(Path.Combine(BepInEx.Paths.PluginPath, "Microsoft.CSharp.dll"));
            // 初始窗口状态
            isWinOpen = openInStart.Value;
        }

        public static void LogInfo(object obj)
        {
            Instance.Logger.LogInfo(obj);
        }

        public static void LogWarning(object obj)
        {
            Instance.Logger.LogWarning(obj);
        }

        public static void LogError(object obj)
        {
            Instance.Logger.LogError(obj);
        }

    }

    [HarmonyPatch(typeof(YSJSONHelper), "InitJSONClassData", MethodType.Normal)]
    public class JsonDataPatch
    {
        [HarmonyPrefix]
        public static void FixMethod()
        {
            Main.Instance.Patch();
        }
    }
}