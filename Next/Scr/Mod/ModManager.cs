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
using JetBrains.Annotations;
using KBEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.StaticFace;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace SkySwordKill.Next.Mod
{
    public static class ModManager
    {
        public delegate void FileHandle(string virtualPath, string filePath);
        
        #region 字段

        public static List<ModConfig> modConfigs = new List<ModConfig>();
        public static MainDataContainer dataContainer;
        public static FieldInfo[] jsonDataFields = typeof(jsonData).GetFields();

        #endregion

        #region 属性

        public static bool ModDataDirty { get; private set; } = false;
        
        #endregion

        #region 回调方法

        public static event Action ModLoadStart;
        public static event Action ModLoadComplete;
        public static event Action ModReload;
        
        private static void OnModLoadStart()
        {
            ModLoadStart?.Invoke();
        }
        
        private static void OnModLoadComplete()
        {
            ModLoadComplete?.Invoke();
        }

        private static void OnModReload()
        {
            ModReload?.Invoke();
        }

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
            {
                OnModReload();
                RestoreBaseData();
                RestoreNextData();
                OnModLoadStart();
                LoadAllMod();
                InitJSONClassData();
                SceneManager.LoadScene("MainMenu");
                ModDataDirty = false;
                OnModLoadComplete();
            }
            sw.Stop();
            Main.LogInfo(string.Format("ModManager.ReloadComplete".I18N(), sw.ElapsedMilliseconds / 1000f));
        }

        public static void FirstLoadAllMod()
        {
            OnModLoadStart();
            LoadAllMod();
            OnModLoadComplete();
        }

        public static void RestoreBaseData()
        {
            // 读取所有继承自IJSONClass的类型
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
            // 反射清空DataDict与DataList
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
            
            // 清空技能缓存
            SkillBox.inst?.clear();
            
            MainDataContainer.CoverMainData(dataContainer);
        }

        public static void RestoreNextData()
        {
            DialogAnalysis.Clear();
            StaticFaceUtils.Clear();
            Main.Instance.luaManager.Reset();
        }

        public static void LoadAllMod()
        {
            modConfigs.Clear();
            Main.Instance.resourcesManager.Init();
            
            Main.LogInfo($"===================" + "ModManager.LoadingModData".I18N() + "=====================");
            jsonData jsonInstance = jsonData.instance;
            DirectoryInfo testModPath = new DirectoryInfo(BepInEx.Paths.GameRootPath+ @"\本地Mod测试");
            List<DirectoryInfo> dirInfoList = new List<DirectoryInfo>();
            foreach (var dir in WorkshopTool.GetAllModDirectory())
            {
                string workshopID = dir.Name;
                bool disable = WorkshopTool.CheckModIsDisable(workshopID);
                if (disable) continue;
                dirInfoList.Add(dir);
            }
            foreach (var dir in testModPath.GetDirectories())
            {
                if (Directory.Exists(dir + @"\plugins\Next"))
                {
                    dirInfoList.Add(new DirectoryInfo(dir + @"\plugins\Next"));
                }
            }
            // 加载元数据
            foreach (DirectoryInfo dirInfo in dirInfoList)
            {
                foreach (DirectoryInfo modDirInfo in dirInfo.GetDirectories("mod*"))
                {
                    Main.LogInfo(string.Format("ModManager.LoadMod".I18N(), modDirInfo.Name));
                    ModConfig item = ModManager.LoadModMetadata(modDirInfo.FullName);
                    ModManager.modConfigs.Add(item);
                }
            }

            // 排序
            modConfigs = SortMod(modConfigs).ToList();
            ResetModPriority();
            
            // 加载Mod数据
            foreach (var modConfig in modConfigs)
            {
                var modSetting = Main.Instance.nextModSetting.GetOrCreateModSetting(modConfig);

                if (!modSetting.enable)
                {
                    modConfig.State = ModState.Disable;
                    continue;
                }
                
                try
                {
                    LoadModData(modConfig);
                }
                catch (Exception e)
                {
                    Main.LogError(string.Format("ModManager.LoadFail".I18N(),modConfig.Path));
                    Main.LogError(e);
                    modConfig.Exception = e;
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
            
            // 重建Buff缓存
            jsonInstance.Buff.Clear();
            jsonInstance.InitBuff();

            // 检查数据
            if (!CheckData.Check())
            {
                Main.LogError(CheckData.log);
            }
            
            Main.Instance.resourcesManager.StartLoadAsset();
        }
        
        private static void InitJSONClassData()
        {
            // 重新初始化所有继承IJSONClass的类型
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

        public static ModConfig LoadModMetadata(string dir)
        {
            var modConfig = GetModConfig(dir);
            modConfig.Path = dir;
            return modConfig;
        }
        
        public static IEnumerable<ModConfig> SortMod(IEnumerable<ModConfig> modEnumerable)
        {
            var mods = modEnumerable.ToArray();
            var nextModSetting = Main.Instance.nextModSetting;

            var modSortList = mods
                .Select(modConfig =>
                {
                    var modId = Path.GetFileNameWithoutExtension(modConfig.Path);
                    var modSetting = nextModSetting.GetOrCreateModSetting(modId);

                    return new { id = modId, setting = modSetting, config = modConfig };
                })
                .OrderBy(data => data.setting.priority)
                .ThenBy(data => data.id)
                .ToArray();

            return modSortList.Select(data => data.config);
        }

        public static void ResetModPriority()
        {
            var index = 0;
            var nextModSetting = Main.Instance.nextModSetting;
            foreach (var modConfig in modConfigs)
            {
                nextModSetting.GetOrCreateModSetting(modConfig).priority = index++;
            }
            Main.Instance.SaveModSetting();
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
                    if (IsBanField(fieldInfo))
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
                
                // 载入Mod Face数据
                LoadCustomFaceData(modConfig.Path);
                
                // 载入Mod Lua数据
                LoadCustomLuaData(modConfig,$"{modConfig.Path}/Lua");

                // 载入ModAsset
                CacheAssetDir($"{modConfig.Path}/Assets");
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

        public static bool IsBanField(FieldInfo fieldInfo)
        {
            if (fieldInfo.Name.StartsWith("_"))
                return true;
            
            if (fieldInfo.Name == "BadWord")
                return true;

            if (fieldInfo.CustomAttributes.FirstOrDefault(attr => attr.AttributeType == typeof(ObsoleteAttribute)) !=
                null)
                return true;

            return false;
        }

        private static ModConfig GetModConfig(string dir)
        {
            ModConfig modConfig = null;
            try
            {
                string filePath = Utility.CombinePaths(dir, $"modConfig.json");
                if (File.Exists(filePath))
                {
                    modConfig = JObject.Parse(File.ReadAllText(filePath)).ToObject<ModConfig>();
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

            modConfig = modConfig ?? new ModConfig();
            modConfig.State = ModState.Unload;

            return modConfig;
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
                var jsonData = LoadJSONObject(filePath);

                foreach (var key in jsonData.keys)
                {
                    var curData = jsonData.GetField(key);
                    if (jsonObject.HasField(key))
                    {
                        // Old data
                        var tagData = jsonObject.GetField(key);
                        foreach (var fieldKey in curData.keys)
                        {
                            tagData.TryAddOrReplace(fieldKey,curData.GetField(fieldKey));
                        }
                    }
                    else
                    {
                        // New data
                        foreach (var fieldKey in dataTemplate.keys)
                        {
                            if (!curData.HasField(fieldKey))
                            {
                                curData.AddField(fieldKey,dataTemplate[fieldKey].Clone());
                                Main.LogWarning(string.Format("ModManager.DataMissingField".I18N(),
                                    fieldInfo.Name, 
                                    fileName, 
                                    fieldKey,
                                    dataTemplate[fieldKey]));
                            
                            }
                        }
                        jsonObject.AddField(key, curData);
                    }
                }

                
                Main.LogInfo(string.Format("ModManager.LoadData".I18N(),$"{dirName}{fileName}.json"));
            }
        }

        public static void PatchJObject(FieldInfo fieldInfo,string filePath, JObject jObject)
        {
            var dataTemplate = jObject.Properties().First().Value;
            
            if (File.Exists(filePath))
            {
                var jsonData = LoadJObject(filePath);
                foreach (var property in jsonData.Properties())
                {
                    if (property.Value.Type !=  JTokenType.Object)
                    {
                        jObject.TryAddOrReplace(property.Name,property.Value);
                        continue;
                    }
                    
                    var curData = (JObject)property.Value;
                    if (jObject.ContainsKey(property.Name))
                    {
                        var tagData = jObject.GetValue(property.Name);
                        if (tagData?.Type == JTokenType.Object)
                        {
                            var tagDataObject = (JObject)tagData;
                            foreach (var field in curData.Properties())
                            {
                                if (tagDataObject.ContainsKey(field.Name))
                                    tagDataObject.Remove(field.Name);
                                tagDataObject.Add(field.Name,curData.GetValue(field.Name));
                            }
                        }
                    }
                    else
                    {
                        if (dataTemplate.Type == JTokenType.Object)
                        {
                            foreach (var field in JObject.FromObject(dataTemplate).Properties())
                            {
                                if (!curData.ContainsKey(field.Name))
                                {
                                    curData.Add(field.Value.DeepClone());
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
                var curData = LoadJSONObject(filePath);
                var key = Path.GetFileNameWithoutExtension(filePath);
                
                if (toJsonObject.HasField(key))
                {
                    var tagData = toJsonObject.GetField(key);
                    foreach (var fieldKey in curData.keys)
                    {
                        tagData.TryAddOrReplace(fieldKey,curData.GetField(fieldKey));
                    }
                }
                else
                {
                    foreach (var fieldKey in dataTemplate.keys)
                    {
                        if (!curData.HasField(fieldKey))
                        {
                            curData.AddField(fieldKey,dataTemplate[fieldKey].Clone());
                            Main.LogWarning(string.Format("ModManager.DataMissingField".I18N(),
                                fieldInfo.Name, 
                                key, 
                                fieldKey,
                                dataTemplate[fieldKey]));
                        }
                    }
                    dicData[key] = curData;
                    toJsonObject.AddField(key, curData);
                }
                
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
                try
                {
                    string json = File.ReadAllText(filePath);
                    JArray.Parse(json).ToObject<List<DialogEventData>>()?.ForEach(TryAddEventData);
                    Main.LogInfo(string.Format("ModManager.LoadData".I18N(),
                        $"{dirName}/{Path.GetFileNameWithoutExtension(filePath)}.json"));
                }
                catch (Exception e)
                {
                    throw new ModLoadException($"文件 {filePath} 加载失败。", e);
                }
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
                try
                {
                    string json = File.ReadAllText(filePath);
                    JArray.Parse(json).ToObject<List<DialogTriggerData>>()?.ForEach(TryAddTriggerData);
                    Main.LogInfo(string.Format("ModManager.LoadData".I18N(),
                        $"{dirName}/{Path.GetFileNameWithoutExtension(filePath)}.json"));
                }
                catch (Exception e)
                {
                    throw new ModLoadException($"文件 {filePath} 加载失败。", e);
                }
            }
        }
        
        private static void LoadCustomFaceData(string dirPath)
        {
            var dirName = "CustomFace";
            var tagDir = Path.Combine(dirPath, dirName);
            if(!Directory.Exists(tagDir))
                return;
            foreach (var filePath in Directory.GetFiles(tagDir))
            {
                try
                {
                    var jObject = LoadJObject(filePath);
                    var faceData = jObject.ToObject<CustomStaticFaceInfo>();
                    StaticFaceUtils.RegisterFace(faceData);
                    Main.LogInfo(string.Format("ModManager.LoadData".I18N(),
                        $"{dirName}/{Path.GetFileNameWithoutExtension(filePath)}.json"));
                }
                catch (Exception e)
                {
                    throw new ModLoadException($"文件 {filePath} 转换失败。", e);
                }
            }
        }

        private static void LoadCustomLuaData(ModConfig modConfig, string rootPath)
        {
            DirectoryHandle("",rootPath, (virtualPath, filePath) =>
            {
                if(Path.GetExtension(filePath) != ".lua")
                    return;
                
                var luaCache = new LuaFileCache()
                {
                    FromMod = modConfig,
                    FilePath = filePath
                        .Replace(@"\", @"/"),
                };
                var luaPath = Path.GetFileNameWithoutExtension(virtualPath)
                    .Replace(@"\", @"/");

                Main.Instance.luaManager.AddLuaCacheFile(luaPath, luaCache);
            });
        }

        public static void CacheAssetDir(string rootPath)
        {
            DirectoryHandle("Assets", rootPath, (virtualPath,filePath) =>
            {
                Main.Instance.resourcesManager.AddAsset(virtualPath, filePath);
            });
        }

        public static void DirectoryHandle(string rootPath,string dirPath,FileHandle fileHandle)
        {
            if(!Directory.Exists(dirPath))
                return;

            foreach (var directory in Directory.GetDirectories(dirPath))
            {
                var name = Path.GetFileNameWithoutExtension(directory);
                DirectoryHandle($"{rootPath}/{name}", directory, fileHandle);
            }

            foreach (var file in Directory.GetFiles(dirPath))
            {
                var fileName = Path.GetFileName(file);
                
                var cachePath = $"{rootPath}/{fileName}";
                fileHandle.Invoke(cachePath, file);
            }
        }

        public static void TryAddEventData(DialogEventData dialogEventData)
        {
            DialogAnalysis.DialogDataDic[dialogEventData.ID] = dialogEventData;
        }
        
        public static void TryAddTriggerData(DialogTriggerData dialogTriggerData)
        {
            DialogAnalysis.DialogTriggerDataDic[dialogTriggerData.ID] = dialogTriggerData;
        }

        public static void ModMoveUp(ref int curIndex)
        {
            if(!TryGetModConfig(curIndex,out var curMod))
                return;
            if(curIndex == 0)
                return;

            modConfigs.RemoveAt(curIndex);
            curIndex -= 1;
            modConfigs.Insert(curIndex, curMod);
            ResetModPriority();
            ModDataDirty = true;
        }
        
        public static void ModMoveDown(ref int curIndex)
        {
            if(!TryGetModConfig(curIndex,out var curMod))
                return;
            if(curIndex == modConfigs.Count-1)
                return;
            
            modConfigs.RemoveAt(curIndex);
            curIndex += 1;
            modConfigs.Insert(curIndex, curMod);
            ResetModPriority();
            ModDataDirty = true;
        }
        
        public static void ModMoveToTop(ref int curIndex)
        {
            if(!TryGetModConfig(curIndex,out var curMod))
                return;
            if(curIndex == 0)
                return;

            modConfigs.RemoveAt(curIndex);
            curIndex = 0;
            modConfigs.Insert(curIndex, curMod);
            ResetModPriority();
            ModDataDirty = true;
        }
        
        public static void ModMoveToBottom(ref int curIndex)
        {
            if(!TryGetModConfig(curIndex,out var curMod))
                return;
            if(curIndex == modConfigs.Count-1)
                return;
            
            modConfigs.RemoveAt(curIndex);
            modConfigs.Add(curMod);
            curIndex = modConfigs.Count-1;
            ResetModPriority();
            ModDataDirty = true;
        }

        public static void ModSetEnable(int curIndex,bool enable)
        {
            if(!TryGetModConfig(curIndex,out var curMod))
                return;
            Main.Instance.nextModSetting.GetOrCreateModSetting(curMod).enable = enable;
            Main.Instance.SaveModSetting();
            ModDataDirty = true;
        }
        
        public static bool ModGetEnable(int curIndex)
        {
            if(!TryGetModConfig(curIndex,out var curMod))
                return true;
            return Main.Instance.nextModSetting.GetOrCreateModSetting(curMod).enable;
        }

        [CanBeNull]
        public static bool TryGetModConfig(int curIndex,out ModConfig modConfig)
        {
            if (curIndex < 0 || curIndex >= modConfigs.Count)
            {
                modConfig = null;
                return false;
            }

            modConfig = modConfigs[curIndex];
            return true;
        }

        public static JSONObject LoadJSONObject(string filePath)
        {
            try
            {
                string data = File.ReadAllText(filePath);
                var jsonData = JSONObject.Create(data);
                return jsonData;
            }
            catch (Exception e)
            {
                throw new ModLoadException($"文件 {filePath} 加载失败。", e);
            }
        }
        
        public static JObject LoadJObject(string filePath)
        {
            try
            {
                string data = File.ReadAllText(filePath);
                var jObject = JObject.Parse(data);
                return jObject;
            }
            catch (Exception e)
            {
                throw new ModLoadException($"文件 {filePath} 加载失败。", e);
            }
        }

        #endregion

        #region 私有方法

        #endregion


    }
}