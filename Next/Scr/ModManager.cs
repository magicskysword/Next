using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using BepInEx;
using HarmonyLib;
using KBEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace SkySwordKill.Next
{
    public static class ModManager
    {
        #region 字段

        public static List<ModConfig> modConfigs = new List<ModConfig>();
        
        #endregion

        #region 属性

        public static Lazy<string> pluginDir =
            new Lazy<string>(() => BepInEx.Utility.CombinePaths(
                BepInEx.Paths.PluginPath, "Next"));

        public static Lazy<string> baseDataDir =
            new Lazy<string>(() => BepInEx.Utility.CombinePaths(
                pluginDir.Value, "Base"));

        public static Lazy<FieldInfo[]> dataField =
            new Lazy<FieldInfo[]>(() => typeof(jsonData).GetFields());

        

        #endregion

        #region 回调方法

        #endregion

        #region 公共方法

        public static void GenerateBaseData()
        {
            Main.LogInfo($"正在生成Base文件。");
            string dirPath = baseDataDir.Value;
            if (Directory.Exists(dirPath))
                Directory.Delete(dirPath, true);
            Directory.CreateDirectory(dirPath);
            jsonData jsonInstance = jsonData.instance;
            foreach (var fieldInfo in dataField.Value)
            {
                if (fieldInfo.Name.StartsWith("_"))
                    continue;

                var value = fieldInfo.GetValue(jsonInstance);

                if (value is JSONObject jsonObject)
                {
                    string filePath = Utility.CombinePaths(dirPath, $"{fieldInfo.Name}.json");
                    File.WriteAllText(filePath, ConvertJson(jsonObject.Print(true)));
                }
                else if (value is JObject jObject)
                {
                    string filePath = Utility.CombinePaths(dirPath, $"{fieldInfo.Name}.json");
                    File.WriteAllText(filePath, jObject.ToString(Formatting.Indented));
                }
                else if (value is jsonData.YSDictionary<string, JSONObject> dicData)
                {
                    string dirPathForData = Utility.CombinePaths(dirPath, fieldInfo.Name);
                    if (!Directory.Exists(dirPathForData))
                        Directory.CreateDirectory(dirPathForData);
                    foreach (var kvp in dicData)
                    {
                        string filePath = Utility.CombinePaths(dirPathForData, $"{kvp.Key}.json");
                        File.WriteAllText(filePath, ConvertJson(kvp.Value.Print(true)));
                    }
                }
                else if (value is JSONObject[] jsonObjects)
                {
                    string dirPathForData = Utility.CombinePaths(dirPath, fieldInfo.Name);
                    if (!Directory.Exists(dirPathForData))
                        Directory.CreateDirectory(dirPathForData);
                    for (int i = 0; i < jsonObjects.Length; i++)
                    {
                        if (jsonObjects[i] == null)
                            continue;
                        string filePath = Utility.CombinePaths(dirPathForData, $"{i}.json");
                        File.WriteAllText(filePath, ConvertJson(jsonObjects[i].Print(true)));
                    }
                }
            }
        }

        public static void LoadAllMod()
        {
            modConfigs.Clear();
            Main.LogInfo($"===================" + "正在读取Mod列表" + "=====================");
            var home = Directory.CreateDirectory(pluginDir.Value);
            jsonData jsonInstance = jsonData.instance;
            foreach (var dir in home.GetDirectories("mod*"))
            {
                try
                {
                    LoadModPatch(jsonInstance, dir.FullName);
                }
                catch (Exception e)
                {
                    Main.LogError($"加载mod出错！{dir.FullName}");
                    Main.LogError(e);
                }
            }
            
            foreach (JSONObject jsonobject in jsonInstance._BuffJsonData.list)
            {
                var key = (int)jsonobject["buffid"].n;
                if (!jsonInstance.Buff.ContainsKey(key))
                {
                    jsonInstance.Buff.Add(key, new Buff(key));
                }
            }

            Main.Instance.resourcesManager.StartLoadAsset();
        }

        public static void LoadModPatch(jsonData jsonInstance, string dir)
        {
            Main.LogInfo($"===================" + "开始载入Mod数据" + "=====================");
            Main.LogInfo($"加载Mod数据：{Path.GetFileNameWithoutExtension(dir)}");
            var modConfig = GetModConfig(dir);
            modConfig.Path = dir;
            Main.logIndent = 1;
            Main.LogInfo($"Mod名称：{modConfig.Name}");
            Main.LogInfo($"Mod作者：{modConfig.Author}");
            Main.LogInfo($"Mod版本：{modConfig.Version}");
            Main.LogInfo($"Mod描述：{modConfig.Description}");
            modConfigs.Add(modConfig);
            try
            {
                // 载入Mod Patch数据
                foreach (var fieldInfo in dataField.Value)
                {
                    if (fieldInfo.Name.StartsWith("_"))
                        continue;

                    var value = fieldInfo.GetValue(jsonInstance);
                    
                    // 普通数据
                    if (value is JSONObject jsonObject)
                    {
                        string filePath = Utility.CombinePaths(dir, $"{fieldInfo.Name}.json");
                        modConfig.jsonPathCache.Add(fieldInfo.Name,filePath);
                        PatchJsonObject(fieldInfo,filePath, jsonObject);
                    }
                    else if (value is JObject jObject)
                    {
                        string filePath = Utility.CombinePaths(dir, $"{fieldInfo.Name}.json");
                        modConfig.jsonPathCache.Add(fieldInfo.Name,filePath);
                        PatchJObject(fieldInfo,filePath, jObject);
                    }
                    else if (value is jsonData.YSDictionary<string, JSONObject> dicData)
                    {
                        string dirPathForData = Utility.CombinePaths(dir, fieldInfo.Name);
                        JSONObject toJsonObject =
                            typeof(jsonData).GetField($"_{fieldInfo.Name}").GetValue(jsonInstance) as JSONObject;
                        modConfig.jsonPathCache.Add(fieldInfo.Name,dirPathForData);
                        PatchDicData(fieldInfo,dirPathForData, dicData, toJsonObject);
                    }
                    // 功能函数配置数据
                    else if (value is JSONObject[] jsonObjects)
                    {
                        string dirPathForData = Utility.CombinePaths(dir, fieldInfo.Name);
                        modConfig.jsonPathCache.Add(fieldInfo.Name,dirPathForData);
                        PatchJsonObjectArray(fieldInfo,dirPathForData, jsonObjects);
                    }
                }
                // 载入Mod Dialog数据
                LoadDialogEventData(dir);
                LoadDialogTriggerData(dir);
                
                // 载入ModAsset
                CacheAssetDir("Assets", $"{dir}/Assets");
            }
            catch (Exception)
            {
                modConfig.Success = false;
                throw;
            }
            modConfig.Success = true;
            Main.logIndent = 0;
            Main.LogInfo($"===================" + "载入Mod数据完成" + "=====================");
        }

        private static ModConfig GetModConfig(string dir)
        {
            try
            {
                string filePath = Utility.CombinePaths(dir, $"modConfig.json");
                if (File.Exists(filePath))
                {
                    return JObject.Parse(File.ReadAllText(filePath)).ToObject<ModConfig>();
                }
                else
                {
                    Main.LogWarning("Mod配置不存在！");
                }
            }
            catch (Exception)
            {
                Main.LogWarning("Mod配置读取错误！");
            }

            return new ModConfig();
        }

        public static void PatchJsonObjectArray(FieldInfo fieldInfo,string dirPathForData, JSONObject[] jsonObjects)
        {
            if (!Directory.Exists(dirPathForData))
                return;
            for (int i = 0; i < jsonObjects.Length; i++)
            {
                if (jsonObjects[i] == null)
                    continue;
                string filePath = Utility.CombinePaths(dirPathForData, $"{i}.json");
                PatchJsonObject(fieldInfo,filePath, jsonObjects[i], $"{Path.GetFileNameWithoutExtension(dirPathForData)}/");
            }
        }

        public static void PatchJsonObject(FieldInfo fieldInfo,string filePath, JSONObject jsonObject, string dirName = "")
        {
            var dataTemplate = jsonObject[0];
            
            if (File.Exists(filePath))
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                string data = File.ReadAllText(filePath);
                var jsonData = JSONObject.Create(data);
                
                
                
                foreach (var key in jsonData.keys)
                {
                    var itemData = jsonData.GetField(key).Copy();
                    
                    foreach (var fieldKey in dataTemplate.keys)
                    {
                        if (!itemData.HasField(fieldKey))
                        {
                            itemData.AddField(fieldKey,dataTemplate[fieldKey].Clone());
                            Main.LogWarning($"数据 {fieldInfo.Name} [{fileName}] 缺少字段 {fieldKey}，" +
                                            $"已用模板对象属性 {dataTemplate[fieldKey]} 替代。");
                        }
                    }

                    jsonObject.TryAddOrReplace(key, itemData);
                }

                
                Main.LogInfo($"载入 {dirName}{fileName}.json");
            }
        }

        public static void PatchJObject(FieldInfo fieldInfo,string filePath, JObject jObject)
        {
            var dataTemplate = jObject.Properties().First().Value;
            
            if (File.Exists(filePath))
            {
                string data = File.ReadAllText(filePath);
                var jsonData = JObject.Parse(data);
                foreach (var property in jsonData.Properties())
                {
                    if (jObject.ContainsKey(property.Name))
                        jObject.Remove(property.Name);
                    
                    var itemData = JObject.FromObject(property.Value);

                    if (dataTemplate.Type == JTokenType.Object)
                    {
                        foreach (var field in JObject.FromObject(dataTemplate).Properties())
                        {
                            if (!itemData.ContainsKey(field.Name))
                            {
                                itemData.Add(field.Value.DeepClone());
                                Main.LogWarning($"数据 {fieldInfo.Name} [{property.Name}] 缺少字段 {field.Name}，" +
                                                $"已用模板对象属性 {field.Value} 替代。");
                            }
                        }
                    }
                    
                    jObject.Add(property.Name, property.Value.DeepClone());
                }

                Main.LogInfo($"载入 {Path.GetFileNameWithoutExtension(filePath)}.json");
            }
        }

        public static void PatchDicData(FieldInfo fieldInfo,string dirPathForData, 
            jsonData.YSDictionary<string, JSONObject> dicData,
            JSONObject toJsonObject)
        {
            if (!Directory.Exists(dirPathForData))
                return;
            var dataTemplate = toJsonObject[0];
            foreach (var filePath in Directory.GetFiles(dirPathForData))
            {
                string data = File.ReadAllText(filePath);
                var jsonData = JSONObject.Create(data);
                var key = Path.GetFileNameWithoutExtension(filePath);

                foreach (var fieldKey in dataTemplate.keys)
                {
                    if (!jsonData.HasField(fieldKey))
                    {
                        jsonData.AddField(fieldKey,dataTemplate[fieldKey].Clone());
                        Main.LogWarning($"数据 {fieldInfo.Name} [{key}] 缺少字段 {fieldKey}，已用模板对象属性 {dataTemplate[fieldKey]} 替代。");
                    }
                }
                
                
                dicData[key] = jsonData;
                toJsonObject.TryAddOrReplace(key, jsonData);
                Main.LogInfo($"载入 {Path.GetFileNameWithoutExtension(dirPathForData)}/" +
                             $"{Path.GetFileNameWithoutExtension(filePath)}.json [{key}]");
            }
        }
        
        public static void LoadDialogEventData(string dirPath)
        {
            var dirName = "DialogEvent";
            var tagDir = Path.Combine(dirPath, dirName);
            if(!Directory.Exists(tagDir))
                return;
            foreach (var filePath in Directory.GetFiles(tagDir))
            {
                string json = File.ReadAllText(filePath);
                JArray.Parse(json).ToObject<List<DialogEventData>>()?.ForEach(TryAddEventData);
                Main.LogInfo($"载入 {dirName}/{Path.GetFileNameWithoutExtension(filePath)}.json");
            }
        }
        
        public static void LoadDialogTriggerData(string dirPath)
        {
            var dirName = "DialogTrigger";
            var tagDir = Path.Combine(dirPath, dirName);
            if(!Directory.Exists(tagDir))
                return;
            foreach (var filePath in Directory.GetFiles(tagDir))
            {
                string json = File.ReadAllText(filePath);
                JArray.Parse(json).ToObject<List<DialogTriggerData>>()?.ForEach(TryAddTriggerData);
                Main.LogInfo($"载入 {dirName}/{Path.GetFileNameWithoutExtension(filePath)}.json");
            }
        }

        public static void CacheAssetDir(string rootPath,string dirPath)
        {
            if(!Directory.Exists(dirPath))
                return;

            foreach (var directory in Directory.GetDirectories(dirPath))
            {
                var name = Path.GetFileNameWithoutExtension(directory);
                CacheAssetDir($"{rootPath}/{name}", directory);
            }

            foreach (var file in Directory.GetFiles(dirPath))
            {
                var fileName = Path.GetFileName(file);
                
                var cachePath = $"{rootPath}/{fileName}";
                Main.Instance.resourcesManager.AddAsset(cachePath,file);
            }
        }

        public static string ConvertJson(string json)
        {
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            string convertSrt = reg.Replace(json,
                delegate(Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
            return convertSrt;
        }

        public static void TryAddOrReplace(this JSONObject jsonObject, string key, JSONObject value)
        {
            var index = jsonObject.keys.IndexOf(key);
            if (index <= -1)
            {
                jsonObject.AddField(key, value.Copy());
            }
            else
            {
                jsonObject.list[index] = value.Copy();
            }
        }
        
        public static void TryAddEventData(DialogEventData dialogEventData)
        {
            DialogAnalysis.dialogDataDic[dialogEventData.id] = dialogEventData;
        }
        
        public static void TryAddTriggerData(DialogTriggerData dialogTriggerData)
        {
            DialogAnalysis.dialogTriggerDataDic[dialogTriggerData.id] = dialogTriggerData;
        }

        #endregion

        #region 私有方法

        #endregion
    }
}