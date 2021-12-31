﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JSONClass;
using MarkerMetro.Unity.WinLegacy.Reflection;
using UnityEngine;
using BindingFlags = System.Reflection.BindingFlags;
using Object = UnityEngine.Object;

namespace SkySwordKill.Next
{
    [BepInPlugin("skyswordkill.plugin.Next", "Next", MOD_VERSION)]
    public partial class Main : BaseUnityPlugin
    {
        public const string MOD_VERSION = "0.2.15";

        public static Main Instance { get; private set; }
        public static int logIndent = 0;
        
        public ConfigEntry<bool> debugMode;
        public ConfigEntry<bool> openInStart;
        public ConfigEntry<KeyCode> debugKeyCode;

        public ResourcesManager resourcesManager;

        private void Awake()
        {
            Init();
        }

        public void PatchJson()
        {
            var watcher = Stopwatch.StartNew();
            ModManager.CloneMainData();
            watcher.Stop();
            LogInfo($"储存数据耗时：{watcher.ElapsedMilliseconds / 1000f} s");
            
            ModManager.LoadAllMod();
            
            // Mod加载完成后显示窗口
            isWinOpen = openInStart.Value;
        }

        private void Init()
        {
            Instance = this;
            /*gameVersion = Config.Bind("General.GameVersion", "游戏版本", "",
                "游戏当前的版本，版本与配置不一致时会重新生成Base文件夹。");*/
            debugKeyCode = Config.Bind("Debug.OpenKeyCode", "调试窗口快捷键", KeyCode.F4,
                "Next插件调试窗口，用于查看当前加载mod以及进行Mod调试（需打开调试模式）。");
            debugMode = Config.Bind("Debug.Mode", "调试模式开关", false,
                "Next插件调试模式开关，打开后才能在调试窗口里看到更多功能。");
            openInStart = Config.Bind("Debug.OpenInStart", "游戏启动时弹出", true,
                "是否在游戏启动时弹出调试窗口。");

            resourcesManager = gameObject.AddComponent<ResourcesManager>();

            // 数据加载Patch
            Harmony.CreateAndPatchAll(typeof(JsonDataPatch));
            Harmony.CreateAndPatchAll(typeof(NpcJieSuanManagerPatch));

            // 资源相关Patch
            Harmony.CreateAndPatchAll(typeof(SkillIconPatch));
            Harmony.CreateAndPatchAll(typeof(ItemUIPatch));

            Harmony.CreateAndPatchAll(typeof(BagActiveSkillGetIconSprite));
            Harmony.CreateAndPatchAll(typeof(BagPassiveSkillGetIconSprite));

            Harmony.CreateAndPatchAll(typeof(ResManagerLoadSpritePatch));
            Harmony.CreateAndPatchAll(typeof(ResManagerLoadTexturePatch));

            // Resources Patch 不成功
            //Harmony.CreateAndPatchAll(typeof(ResourcesPatch));

            // 加载运行时脚本所需DLL
            string nextLibDirPath = Path.Combine(BepInEx.Paths.PluginPath, "NextLib");
            Assembly.LoadFrom(Path.Combine(nextLibDirPath, "Microsoft.CSharp.dll"));

            DialogAnalysis.Init();

            // 初始窗口状态
            RefreshWinRect();
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
    

    [HarmonyPatch(typeof(YSJSONHelper), "InitJSONClassData")]
    public class JsonDataPatch
    {
        [HarmonyPrefix]
        public static void FixMethod()
        {
            Main.Instance.PatchJson();
        }
    }

    [HarmonyPatch(typeof(NpcJieSuanManager),"InitCyData")]
    public class NpcJieSuanManagerPatch
    {
        [HarmonyPrefix]
        public static void FixMethod(NpcJieSuanManager __instance)
        {
            Main.LogInfo("重新Patch Npc数据");
            jsonData jsonInstance = jsonData.instance;
            var fieldInfo = typeof(jsonData).GetField("AvatarJsonData");
            foreach (var modConfig in ModManager.modConfigs)
            {
                if (modConfig.jsonPathCache.TryGetValue("AvatarJsonData",out var path))
                {
                    ModManager.PatchJsonObject(fieldInfo,path,jsonInstance.AvatarJsonData);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Resources), "Load", new Type[] {typeof(string),typeof(Type)})]
    public class ResourcesPatch
    {
        [HarmonyPrefix]
        public static bool FixMethod(string path,Type systemTypeInstance,ref Object __result)
        {
            Main.LogInfo($"读取资源：<{systemTypeInstance}>({path}");
            if (Main.Instance.resourcesManager.TryGetAsset(path, out var asset))
            {
                __result = asset;
                return true;
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(GUIPackage.Skill), "InitImage")]
    public class SkillIconPatch
    {
        public static int GetStaticSkillIconByKey(int key)
        {
            if (key < 0)
                return -1;
            else
            {
                var staticSkillJsonData = StaticSkillJsonData.DataDict[key];
                return staticSkillJsonData.icon > 0 ? staticSkillJsonData.icon : staticSkillJsonData.Skill_ID;
            }
        }

        public static int GetSkillIconByKey(int key)
        {
            if (key < 0)
                return -1;
            else
            {
                var staticSkillJsonData = _skillJsonData.DataDict[key];
                return staticSkillJsonData.icon > 0 ? staticSkillJsonData.icon : staticSkillJsonData.Skill_ID;
            }
        }

        [HarmonyPostfix]
        public static void Postfix(GUIPackage.Skill __instance)
        {
            if (__instance.IsStaticSkill)
            {
                // 功法图标
                var staticSkillIDByKey = GetStaticSkillIconByKey(__instance.skill_ID);
                var path = $"StaticSkill Icon/{staticSkillIDByKey}";
                if (Main.Instance.resourcesManager.TryGetAsset($"Assets/{path}.png", asset =>
                {
                    if (asset is Texture2D texture)
                    {
                        __instance.skill_Icon = texture;
                        __instance.skillIconSprite = Main.Instance.resourcesManager.GetSpriteCache(texture);
                    }
                }))
                {
                    Main.LogInfo($"功法 [{__instance.skill_ID}] 图标加载成功");
                }
                else
                {
                    Texture2D exists = Resources.Load<Texture2D>(path);
                    if (exists)
                    {
                        __instance.skill_Icon = exists;
                    }
                    else
                    {
                        __instance.skill_Icon = Resources.Load<Texture2D>("StaticSkill Icon/0");
                    }
                    Sprite exists2 = Resources.Load<Sprite>(path);
                    if (exists2)
                    {
                        __instance.skillIconSprite = exists2;
                    }
                    else
                    {
                        __instance.skillIconSprite = Resources.Load<Sprite>("StaticSkill Icon/0");
                    }
                }
            }
            else
            {
                // 神通图标
                var path = $"Skill Icon/{GetSkillIconByKey(__instance.skill_ID)}";
                if (Main.Instance.resourcesManager.TryGetAsset($"Assets/{path}.png", asset =>
                {
                    if (asset is Texture2D texture)
                    {
                        __instance.skill_Icon = texture;
                        __instance.skillIconSprite = Main.Instance.resourcesManager.GetSpriteCache(texture);
                    }
                }))
                {
                    Main.LogInfo($"技能 [{__instance.skill_ID}] 图标加载成功");
                }
                else
                {
                    Texture2D exists = Resources.Load<Texture2D>(path);
                    if (exists)
                    {
                        __instance.skill_Icon = exists;
                    }
                    else
                    {
                        __instance.skill_Icon = Resources.Load<Texture2D>("Skill Icon/0");
                    }
                    Sprite exists2 = Resources.Load<Sprite>(path);
                    if (exists2)
                    {
                        __instance.skillIconSprite = exists2;
                    }
                    else
                    {
                        __instance.skillIconSprite = Resources.Load<Sprite>("Skill Icon/0");
                    }
                };
            }
        }
    }

    [HarmonyPatch(typeof(GUIPackage.item), "InitImage")]
    public class ItemUIPatch
    {
        public static int GetItemIconByKey(_ItemJsonData itemJsonData)
        {
            return itemJsonData.ItemIcon > 0 ? itemJsonData.ItemIcon : itemJsonData.id;
        }

        [HarmonyPostfix]
        public static void Postfix(GUIPackage.item __instance)
        {
            int id = __instance.itemID;
            if (id == -1) return;
            if (!_ItemJsonData.DataDict.ContainsKey(id))
            {
                return;
            }
            _ItemJsonData itemJsonData = _ItemJsonData.DataDict[id];
            var path = $"Item Icon/{GetItemIconByKey(itemJsonData)}";
            if (Main.Instance.resourcesManager.TryGetAsset($"Assets/{path}.png", asset =>
            {
                if (asset is Texture2D texture)
                {
                    __instance.itemIcon = texture;
                    __instance.itemIconSprite = Main.Instance.resourcesManager.GetSpriteCache(texture);
                }
            }))
            {
                Main.LogInfo($"物品 [{__instance.itemID}] 图标加载成功");
            }
        }
    }

    [HarmonyPatch(typeof(Bag.ActiveSkill),"GetIconSprite")]
    public class BagActiveSkillGetIconSprite
    {
        [HarmonyPrefix]
        public static bool LoadSprite(Bag.ActiveSkill __instance,ref Sprite __result)
        {
            __result = ResManager.inst.LoadSprite("Skill Icon/" + SkillIconPatch.GetSkillIconByKey(__instance.Id));
            return false;
        }
    }

    [HarmonyPatch(typeof(Bag.PassiveSkill),"GetIconSprite")]
    public class BagPassiveSkillGetIconSprite
    {
        [HarmonyPrefix]
        public static bool LoadSprite(Bag.PassiveSkill __instance,ref Sprite __result)
        {
            __result = ResManager.inst.LoadSprite("StaticSkill Icon/" + SkillIconPatch.GetStaticSkillIconByKey(__instance.Id));
            return false;
        }
    }

    [HarmonyPatch(typeof(ResManager),"LoadTexture2D")]
    public class ResManagerLoadTexturePatch
    {
        [HarmonyPrefix]
        public static bool LoadTexture2D(ResManager __instance,string path,ref Texture2D __result)
        {
            if (Main.Instance.resourcesManager.TryGetAsset($"Assets/{path}.png", out var asset))
            {
                if (asset is Texture2D texture)
                {
                    __result = texture;
                    return false;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(ResManager),"LoadSprite")]
    public class ResManagerLoadSpritePatch
    {
        [HarmonyPrefix]
        public static bool LoadSprite(ResManager __instance,string path,ref Sprite __result)
        {
            if (Main.Instance.resourcesManager.TryGetAsset($"Assets/{path}.png", out var asset))
            {
                if (asset is Texture2D texture)
                {
                    __result = Main.Instance.resourcesManager.GetSpriteCache(texture);
                    return false;
                }
            }

            return true;
        }
    }
}