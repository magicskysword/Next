using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace SkySwordKill.Next
{
    [BepInPlugin("skyswordkill.plugin.Next", "Next", MOD_VERSION)]
    public partial class Main : BaseUnityPlugin
    {
        public const string MOD_VERSION = "0.2.4";
        
        public static Main Instance { get; private set; }
        public static int logIndent = 0;

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
                ModManager.GenerateBaseData();
            }

            gameVersion.Value = Application.version;
            ModManager.LoadAllMod();
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
            
            Harmony.CreateAndPatchAll(typeof(JsonDataPatch));
            // 加载运行时脚本所需DLL
            string nextLibDirPath = Path.Combine(BepInEx.Paths.PluginPath, "NextLib");
            Assembly.LoadFrom(Path.Combine(nextLibDirPath, "Microsoft.CSharp.dll"));

            DialogAnalysis.Init();

            // 初始窗口状态
            isWinOpen = openInStart.Value;
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