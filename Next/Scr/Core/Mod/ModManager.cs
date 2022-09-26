using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using JetBrains.Annotations;
using KBEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FCanvas;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.Next.StaticFace;
using SkySwordKill.NextModEditor.Mod;
using UnityEngine.SceneManagement;

namespace SkySwordKill.Next.Mod;

public static class ModManager
{
        
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

    public static void GenerateBaseData(Action onComplete = null)
    {
        WindowWaitDialog.CreateDialog("提示", "正在导出数据...", 1f,
            context =>
            {
                try
                {
                    GenerateBaseDataWithoutGUI();
                }
                catch (Exception e)
                {
                    Main.LogError(e);
                    context.Exception = e;
                }
            }, 
            context =>
            {
                if (context.Exception != null)
                {
                    WindowConfirmDialog.CreateDialog("提示", $"数据导出失败！错误信息：\n{context.Exception}", false, () =>
                    {
                        onComplete?.Invoke();
                    });
                }
                else
                {
                    WindowConfirmDialog.CreateDialog("提示", "数据导出成功！", false, () =>
                    {
                        onComplete?.Invoke();
                    });
                }
            });
    }

    public static void GenerateBaseDataWithoutGUI()
    {
        Main.LogInfo("ModManager.GenerateBaseData".I18N());

        var sw = Stopwatch.StartNew();

        MainDataContainer.ExportMainData(dataContainer, Main.PathBaseDataDir.Value);
        var dataCost = sw.ElapsedMilliseconds;
            
        FFlowchartTools.ExportAllFungusFlowchart(Main.PathBaseFungusDataDir.Value);
        var fungusCost = sw.ElapsedMilliseconds - dataCost;

        sw.Stop();
        Main.LogInfo($"所有数据导出完毕，总耗时 {sw.ElapsedMilliseconds / 1000f} s");
        Main.LogIndent += 1;
        Main.LogInfo($"Data导出耗时\t：{dataCost / 1000f} s");
        Main.LogInfo($"Fungus导出耗时\t：{fungusCost / 1000f} s");
        Main.LogIndent -= 1;
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
        var watcher = Stopwatch.StartNew();
        // 缓存游戏数据
        CloneMainData();
        watcher.Stop();
        Main.LogInfo($"储存数据耗时：{watcher.ElapsedMilliseconds / 1000f} s");
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
        Main.Lua.Reset();
        Main.FGUI.Reset();
        Main.Res.Reset();
        Main.FPatch.Reset();
    }

    public static void LoadAllMod()
    {
        modConfigs.Clear();

        Main.LogInfo($"===================" + "ModManager.LoadingModData".I18N() + "=====================");
        jsonData jsonInstance = jsonData.instance;
        DirectoryInfo testModPath = new DirectoryInfo(Main.PathLocalModsDir.Value);
        List<DirectoryInfo> dirInfoList = new List<DirectoryInfo>();
        // 加载工坊模组
        foreach (var dir in WorkshopTool.GetAllModDirectory())
        {
            string workshopID = dir.Name;
            bool disable = WorkshopTool.CheckModIsDisable(workshopID);
            if (disable)
            {
                Main.LogInfo($"{workshopID}是关闭的，跳过");
                continue;
            }
            if (Directory.Exists(dir + @"\plugins\Next"))
            {
                dirInfoList.Add(new DirectoryInfo(dir + @"\plugins\Next"));
            }
        }
        // 加载本地模组
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
                ModConfig item = LoadModMetadata(modDirInfo.FullName);
                modConfigs.Add(item);
            }
        }

        // 排序
        modConfigs = SortMod(modConfigs).ToList();
        ResetModPriority();
            
        // 加载Mod数据
        foreach (var modConfig in modConfigs)
        {
            var modSetting = Main.I.NextModSetting.GetOrCreateModSetting(modConfig);

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
        return modConfig;
    }
        
    public static IEnumerable<ModConfig> SortMod(IEnumerable<ModConfig> modEnumerable)
    {
        var mods = modEnumerable.ToArray();
        var nextModSetting = Main.I.NextModSetting;

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
        var nextModSetting = Main.I.NextModSetting;
        foreach (var modConfig in modConfigs)
        {
            nextModSetting.GetOrCreateModSetting(modConfig).priority = index++;
        }
        Main.I.SaveModSetting();
    }

    private static void LoadModData(ModConfig modConfig)
    {
        Main.LogInfo($"===================" + "ModManager.StartLoadMod".I18N() + "=====================");
        // 获取版本地址
        var modConfigDir = modConfig.GetConfigDir();
        var modDataDir = modConfig.GetDataDir();
        var modNDataDir = modConfig.GetNDataDir();
            
        Main.LogInfo($"{"Mod.Directory".I18N()} : {Path.GetFileNameWithoutExtension(modConfig.Path)}");
        Main.LogIndent = 1;
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
                try
                {
                    if (IsBanField(fieldInfo))
                        continue;

                    var value = fieldInfo.GetValue(jsonInstance);

                    // 普通数据
                    if (value is JSONObject jsonObject)
                    {
                        string filePath = Utility.CombinePaths(modDataDir, $"{fieldInfo.Name}.json");
                        modConfig.jsonPathCache.Add(fieldInfo.Name, filePath);
                        PatchJsonObject(fieldInfo, filePath, jsonObject);
                    }
                    else if (value is JObject jObject)
                    {
                        string filePath = Utility.CombinePaths(modDataDir, $"{fieldInfo.Name}.json");
                        modConfig.jsonPathCache.Add(fieldInfo.Name, filePath);
                        PatchJObject(fieldInfo, filePath, jObject);
                    }
                    else if (value is jsonData.YSDictionary<string, JSONObject> dicData)
                    {
                        string dirPathForData = Utility.CombinePaths(modDataDir, fieldInfo.Name);
                        JSONObject toJsonObject =
                            typeof(jsonData).GetField($"_{fieldInfo.Name}").GetValue(jsonInstance) as JSONObject;
                        modConfig.jsonPathCache.Add(fieldInfo.Name, dirPathForData);
                        PatchDicData(fieldInfo, dirPathForData, dicData, toJsonObject);
                    }
                    // 功能函数配置数据
                    else if (value is JSONObject[] jsonObjects)
                    {
                        string dirPathForData = Utility.CombinePaths(modDataDir, fieldInfo.Name);
                        modConfig.jsonPathCache.Add(fieldInfo.Name, dirPathForData);
                        PatchJsonObjectArray(fieldInfo, dirPathForData, jsonObjects);
                    }
                }
                catch (Exception e)
                {
                    throw new ModLoadException($"加载 Data【{fieldInfo.Name}】数据失败", e);
                }
            }

            // 载入Mod Dialog数据
            LoadDialogEventData(modNDataDir);
            LoadDialogTriggerData(modNDataDir);

            // 载入Mod Face数据
            LoadCustomFaceData(modNDataDir);
                
            // 载入Mod FungusPatch数据
            LoadFPatchData(modNDataDir);
                
            // 载入Mod Lua数据
            LoadCustomLuaData(modConfig,$"{modConfig.Path}/Lua");

            // 载入ModAsset
            Main.Res.CacheAssetDir($"{modConfig.Path}/Assets");
        }
            
        catch (Exception)
        {
            modConfig.State = ModState.LoadFail;
            throw;
        }

        modConfig.State = ModState.LoadSuccess;
        Main.LogIndent = 0;
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
            modConfig = ModConfig.Load(dir);
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
        try
        {
            JSONObject dataTemplate = null;
            if(jsonObject.Count > 0)
            {
                dataTemplate = jsonObject[0];
            }

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
                        if(dataTemplate != null)
                        {
                            foreach (var fieldKey in dataTemplate.keys)
                            {
                                if (!curData.HasField(fieldKey))
                                {
                                    curData.AddField(fieldKey, dataTemplate[fieldKey].Clone());
                                    Main.LogWarning(string.Format("ModManager.DataMissingField".I18N(),
                                        fieldInfo.Name,
                                        fileName,
                                        fieldKey,
                                        dataTemplate[fieldKey]));

                                }
                            }
                        }
                        jsonObject.AddField(key, curData);
                    }
                }
                Main.LogInfo(string.Format("ModManager.LoadData".I18N(),$"{dirName}{fileName}.json"));
            }
        }
        catch (Exception e)
        {
            throw new ModLoadException($"文件【{filePath}】加载失败", e);
        }
    }

    public static void PatchJObject(FieldInfo fieldInfo,string filePath, JObject jObject)
    {
        try
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
        catch (Exception e)
        {
            throw new ModLoadException($"文件【{filePath}】加载失败", e);
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
            try
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
            catch (Exception e)
            {
                throw new ModLoadException($"文件 {filePath} 解析失败", e);
            }
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
                throw new ModLoadException($"DialogEvent {filePath} 加载失败。", e);
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
                throw new ModLoadException($"DialogTrigger {filePath} 加载失败。", e);
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
                throw new ModLoadException($"StaticFace {filePath} 转换失败。", e);
            }
        }
    }
        
    private static void LoadFPatchData(string dirPath)
    {
        var dirName = "FungusPatch";
        var tagDir = Path.Combine(dirPath, dirName);
        if(!Directory.Exists(tagDir))
            return;
        foreach (var filePath in Directory.GetFiles(tagDir))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var fPatches = JArray.Parse(json).ToObject<List<FPatch>>();
                foreach (var fPatch in fPatches)
                {
                    Main.FPatch.AddPatch(fPatch);
                }
                Main.LogInfo(string.Format("ModManager.LoadData".I18N(),
                    $"{dirName}/{Path.GetFileNameWithoutExtension(filePath)}.json"));
            }
            catch (Exception e)
            {
                throw new ModLoadException($"FPatch {filePath} 加载失败。", e);
            }
        }
    }

    private static void LoadCustomLuaData(ModConfig modConfig, string rootPath)
    {
        Main.Res.DirectoryHandle("",rootPath, (virtualPath, filePath) =>
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

            try
            {
                Main.Lua.AddLuaCacheFile(luaPath, luaCache);
            }
            catch (Exception e)
            {
                throw new ModLoadException($"Lua {luaPath} 加载失败。", e);
            }
        });
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
        Main.I.NextModSetting.GetOrCreateModSetting(curMod).enable = enable;
        Main.I.SaveModSetting();
        ModDataDirty = true;
    }
        
    public static bool ModGetEnable(int curIndex)
    {
        if(!TryGetModConfig(curIndex,out var curMod))
            return true;
        return Main.I.NextModSetting.GetOrCreateModSetting(curMod).enable;
    }
        
        
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