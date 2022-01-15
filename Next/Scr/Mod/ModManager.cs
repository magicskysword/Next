using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
using UnityEngine.SceneManagement;

namespace SkySwordKill.Next.Mod
{
    public static class ModManager
    {
        #region 字段

        public static List<ModConfig> modConfigs = new List<ModConfig>();
        public static MainDataContainer dataContainer;

        #endregion

        #region 属性

        public static FieldInfo[] jsonDataFields = typeof(jsonData).GetFields();
        
        #endregion

        #region 回调方法

        #endregion

        #region 公共方法

        public static void CloneMainData()
        {
            dataContainer = MainDataContainer.CloneMainData();
        }

        public static void GenerateBaseData()
        {
            Main.LogInfo("ModManager.GenerateBaseData".I18N());

            var sw = Stopwatch.StartNew();
            
            string dirPath = Main.pathBaseDataDir.Value;
            if (Directory.Exists(dirPath))
                Directory.Delete(dirPath, true);
            Directory.CreateDirectory(dirPath);

            foreach (var pair in dataContainer.dataJObjects)
            {
                string filePath = Utility.CombinePaths(dirPath, $"{pair.Key}.json");
                File.WriteAllText(filePath, pair.Value.ToString(Formatting.Indented));
            }
            
            foreach (var pair in dataContainer.dataJSONObjects)
            {
                string filePath = Utility.CombinePaths(dirPath, $"{pair.Key}.json");
                File.WriteAllText(filePath, pair.Value.Print(true).DecodeJsonUnicode());
            }
            
            foreach (var pair in dataContainer.dataYSDics)
            {
                string dirPathForData = Utility.CombinePaths(dirPath, pair.Key);
                if (!Directory.Exists(dirPathForData))
                    Directory.CreateDirectory(dirPathForData);
                foreach (var kvp in pair.Value)
                {
                    string filePath = Utility.CombinePaths(dirPathForData, $"{kvp.Key}.json");
                    File.WriteAllText(filePath, kvp.Value.Print(true).DecodeJsonUnicode());
                }
            }
            
            foreach (var pair in dataContainer.dataJSONObjectArrays)
            {
                string dirPathForData = Utility.CombinePaths(dirPath, pair.Key);
                if (!Directory.Exists(dirPathForData))
                    Directory.CreateDirectory(dirPathForData);
                var jsonObjects = pair.Value;
                for (int i = 0; i < jsonObjects.Length; i++)
                {
                    if (jsonObjects[i] == null)
                        continue;
                    string filePath = Utility.CombinePaths(dirPathForData, $"{i}.json");
                    File.WriteAllText(filePath, jsonObjects[i].Print(true).DecodeJsonUnicode());
                }
            }
            
            sw.Stop();
            Main.LogInfo($"Base导出完毕，耗时 {sw.ElapsedMilliseconds / 1000f} s");
        }

        public static void ReloadAllMod()
        {
            Main.LogInfo($"ModManager.StartReloadMod".I18N());
            var sw = Stopwatch.StartNew();
            RestoreBaseData();
            LoadAllMod();
            InitJSONClassData();
            SceneManager.LoadScene("MainMenu");
            sw.Stop();
            Main.LogInfo(string.Format("ModManager.ReloadComplete".I18N(), sw.ElapsedMilliseconds / 1000f));
        }

        private static void InitJSONClassData()
        {
            Type[] types = Assembly.GetAssembly(typeof(IJSONClass)).GetTypes();
            List<Type> list = new List<Type>();
            foreach (Type type in types)
            {
                if (!type.IsInterface)
                {
                    Type[] interfaces = type.GetInterfaces();
                    for (int j = 0; j < interfaces.Length; j++)
                    {
                        if (interfaces[j] == typeof(IJSONClass))
                        {
                            list.Add(type);
                        }
                    }
                }
            }
            foreach (Type type2 in list)
            {
                MethodInfo method = type2.GetMethod("InitDataDict");
                if (method != null)
                {
                    method.Invoke(null, null);
                }
            }
        }

        public static void RestoreBaseData()
        {
            Type[] types = Assembly.GetAssembly(typeof(IJSONClass)).GetTypes();
            List<Type> list = new List<Type>();
            foreach (Type type in types)
            {
                if (!type.IsInterface)
                {
                    Type[] interfaces = type.GetInterfaces();
                    for (int j = 0; j < interfaces.Length; j++)
                    {
                        if (interfaces[j] == typeof(IJSONClass))
                        {
                            list.Add(type);
                        }
                    }
                }
            }
            foreach (Type jsonType in list)
            {
                var dataDic = 
                    jsonType
                        .GetField("DataDict", BindingFlags.Static | BindingFlags.Public)
                        ?.GetValue(null) as IDictionary;
                dataDic?.Clear();
                
                var dataList = 
                    jsonType
                        .GetField("DataList", BindingFlags.Static | BindingFlags.Public)
                        ?.GetValue(null) as IList;
                dataList?.Clear();
            }
            
            MainDataContainer.CoverMainData(dataContainer);
        }
        
        public static void LoadAllMod()
        {
            modConfigs.Clear();
            Main.Instance.resourcesManager.Init();
            
            Main.LogInfo($"===================" + "ModManager.LoadingModData".I18N() + "=====================");
            var home = Directory.CreateDirectory(Main.pathModsDir.Value);
            jsonData jsonInstance = jsonData.instance;
            var modDirectories = home.GetDirectories("mod*");

            // 加载元数据
            foreach (var dir in modDirectories)
            {
                Main.LogInfo(string.Format("ModManager.LoadMod".I18N(),dir.Name));
                var modConfig = LoadModMetadata(dir.FullName);
                modConfigs.Add(modConfig);
            }
            
            // 加载Mod数据
            foreach (var modConfig in modConfigs)
            {
                try
                {
                    LoadModData(modConfig);
                }
                catch (Exception e)
                {
                    Main.LogError(string.Format("ModManager.LoadFail".I18N(),modConfig.Path));
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

        public static ModConfig LoadModMetadata(string dir)
        {
            var modConfig = GetModConfig(dir);
            modConfig.Path = dir;
            return modConfig;
        }

        private static void LoadModData(ModConfig modConfig)
        {
            Main.LogInfo($"===================" + "ModManager.StartLoadMod".I18N() + "=====================");
            Main.LogInfo($"{"Mod.Directory".I18N()} : {Path.GetFileNameWithoutExtension(modConfig.Path)}");
            Main.logIndent = 1;
            Main.LogInfo($"{"Mod.Name".I18N()} : {modConfig.Name}");
            Main.LogInfo($"{"Mod.Author".I18N()} : {modConfig.Author}");
            Main.LogInfo($"{"Mod.Version".I18N()} : {modConfig.Version}");
            Main.LogInfo($"{"Mod.Description".I18N()} : {modConfig.Description}");
            try
            {
                jsonData jsonInstance = jsonData.instance;
                modConfig.State = ModState.Loading;
                // 载入Mod Patch数据
                foreach (var fieldInfo in jsonDataFields)
                {
                    if (fieldInfo.Name.StartsWith("_"))
                        continue;

                    var value = fieldInfo.GetValue(jsonInstance);

                    // 普通数据
                    if (value is JSONObject jsonObject)
                    {
                        string filePath = Utility.CombinePaths(modConfig.Path, $"{fieldInfo.Name}.json");
                        modConfig.jsonPathCache.Add(fieldInfo.Name, filePath);
                        PatchJsonObject(fieldInfo, filePath, jsonObject);
                    }
                    else if (value is JObject jObject)
                    {
                        string filePath = Utility.CombinePaths(modConfig.Path, $"{fieldInfo.Name}.json");
                        modConfig.jsonPathCache.Add(fieldInfo.Name, filePath);
                        PatchJObject(fieldInfo, filePath, jObject);
                    }
                    else if (value is jsonData.YSDictionary<string, JSONObject> dicData)
                    {
                        string dirPathForData = Utility.CombinePaths(modConfig.Path, fieldInfo.Name);
                        JSONObject toJsonObject =
                            typeof(jsonData).GetField($"_{fieldInfo.Name}").GetValue(jsonInstance) as JSONObject;
                        modConfig.jsonPathCache.Add(fieldInfo.Name, dirPathForData);
                        PatchDicData(fieldInfo, dirPathForData, dicData, toJsonObject);
                    }
                    // 功能函数配置数据
                    else if (value is JSONObject[] jsonObjects)
                    {
                        string dirPathForData = Utility.CombinePaths(modConfig.Path, fieldInfo.Name);
                        modConfig.jsonPathCache.Add(fieldInfo.Name, dirPathForData);
                        PatchJsonObjectArray(fieldInfo, dirPathForData, jsonObjects);
                    }
                }

                // 载入Mod Dialog数据
                LoadDialogEventData(modConfig.Path);
                LoadDialogTriggerData(modConfig.Path);

                // 载入ModAsset
                CacheAssetDir("Assets", $"{modConfig.Path}/Assets");
            }
            catch (Exception)
            {
                modConfig.State = ModState.LoadFail;
                throw;
            }

            modConfig.State = ModState.LoadSuccess;
            Main.logIndent = 0;
            Main.LogInfo($"===================" + "ModManager.LoadModComplete".I18N() + "=====================");
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
                    Main.LogWarning("ModManager.ModConfigDontExist".I18N());
                }
            }
            catch (Exception)
            {
                Main.LogWarning($"ModManager.ModConfigLoadFail".I18N());
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
                            Main.LogWarning(string.Format("ModManager.DataMissingField".I18N(),
                                fieldInfo.Name, 
                                fileName, 
                                fieldKey,
                                dataTemplate[fieldKey]));
                            
                        }
                    }

                    jsonObject.TryAddOrReplace(key, itemData);
                }

                
                Main.LogInfo(string.Format("ModManager.LoadData".I18N(),$"{dirName}{fileName}.json"));
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
                                Main.LogWarning(string.Format("ModManager.DataMissingField".I18N(),
                                    fieldInfo.Name, 
                                    property.Name, 
                                    field.Name,
                                    field.Value));
                            }
                        }
                    }
                    
                    jObject.Add(property.Name, property.Value.DeepClone());
                }
                
                Main.LogInfo(string.Format("ModManager.LoadData".I18N(),
                    $"{Path.GetFileNameWithoutExtension(filePath)}.json"));
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
                        Main.LogWarning(string.Format("ModManager.DataMissingField".I18N(),
                            fieldInfo.Name, 
                            key, 
                            fieldKey,
                            dataTemplate[fieldKey]));
                    }
                }
                
                
                dicData[key] = jsonData;
                toJsonObject.TryAddOrReplace(key, jsonData);
                Main.LogInfo(string.Format("ModManager.LoadData".I18N(),
                    $"{Path.GetFileNameWithoutExtension(dirPathForData)}/{Path.GetFileNameWithoutExtension(filePath)}.json [{key}]"));
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
                Main.LogInfo(string.Format("ModManager.LoadData".I18N(),
                    $"{dirName}/{Path.GetFileNameWithoutExtension(filePath)}.json"));
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
                Main.LogInfo(string.Format("ModManager.LoadData".I18N(),
                    $"{dirName}/{Path.GetFileNameWithoutExtension(filePath)}.json"));
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