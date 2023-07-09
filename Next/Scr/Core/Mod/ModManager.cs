using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using BepInEx;
using Cysharp.Threading.Tasks;
using KBEngine;
using Newtonsoft.Json.Linq;
using script.Steam;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FCanvas;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.Next.StaticFace;
using UnityEngine.SceneManagement;

namespace SkySwordKill.Next.Mod;

public static class ModManager
{
        
    #region 字段

    public static List<ModGroup> modGroups = new List<ModGroup>();
    public static MainDataContainer dataContainer;
    public static FieldInfo[] jsonDataFields = typeof(jsonData).GetFields();

    #endregion

    #region 属性

    #endregion

    #region 回调方法

    public static event Action ModLoadStart;
    public static event Action ModLoadComplete;
    public static event Action ModReload;
    public static event Action ModSettingChanged;
    private static Dictionary<string, ICustomSetting> _customSetting = new Dictionary<string, ICustomSetting>();
    private static bool _buildCacheSuccess = false;
        
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
    
    private static void OnModSettingChanged()
    {
        ModSettingChanged?.Invoke();
    }

    #endregion

    #region 公共方法

    public static void CloneMainData()
    {
        dataContainer = MainDataContainer.CloneMainData();
    }

    public static void GenerateBaseData(Action onComplete = null)
    {
        WindowWaitDialog.CreateDialogAsync("提示", "正在导出基础数据...", 1f,
            (callback, context) =>
            {
                UniTask.Create(async () =>
                {
                    await UniTask.SwitchToThreadPool();
                    try
                    {
                        await GenerateBaseDataWithoutGUIAsync();
                    }
                    catch (Exception e)
                    {
                        Main.LogError(e);
                        context.Exception = e;
                    }
                    finally
                    {
                        await UniTask.SwitchToMainThread();
                        callback?.Invoke();
                    }
                });
            }, 
            context =>
            {
                if (context.Exception != null)
                {
                    WindowConfirmDialog.CreateDialog("提示".I18NTodo(), 
                        $"数据导出失败！错误信息：".I18NTodo() + $"\n{context.Exception}", false);
                }
                else
                {
                    WindowConfirmDialog.CreateDialog("提示".I18NTodo(), "数据导出成功！".I18NTodo(), false, () =>
                    {
                        onComplete?.Invoke();
                    });
                }
            });
    }

    public static void GenerateCurrentSceneFungusData(Action onComplete = null)
    {
        WindowWaitDialog.CreateDialog("提示", "正在导出Fungus数据...", 1f,
            context =>
            {
                try
                {
                    FFlowchartTools.ExportCurrentSceneFungusFlowchart(Main.PathBaseFungusDataDir.Value);
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
                    WindowConfirmDialog.CreateDialog("提示".I18NTodo(), 
                        $"数据导出失败！错误信息：".I18NTodo() + $"\n{context.Exception}", false);
                }
                else
                {
                    WindowConfirmDialog.CreateDialog("提示".I18NTodo(), "数据导出成功！".I18NTodo(), false, () =>
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
        Main.LogInfo(string.Format("所有数据导出完毕，总耗时\t {0} s".I18NTodo(), sw.ElapsedMilliseconds / 1000f));
        Main.LogIndent += 1;
        Main.LogInfo(string.Format("Data导出耗时\t：{0} s".I18NTodo(), dataCost / 1000f));
        Main.LogInfo(string.Format("Fungus导出耗时\t：{0} s".I18NTodo(), fungusCost / 1000f));
        Main.LogIndent -= 1;
    }
    
    public static async UniTask GenerateBaseDataWithoutGUIAsync()
    {
        Main.LogInfo("ModManager.GenerateBaseData".I18N());

        var sw = Stopwatch.StartNew();

        await UniTask.SwitchToThreadPool();
        
        MainDataContainer.ExportMainData(dataContainer, Main.PathBaseDataDir.Value);
        var dataCost = sw.ElapsedMilliseconds;

        await UniTask.SwitchToMainThread();
            
        FFlowchartTools.ExportAllFungusFlowchart(Main.PathBaseFungusDataDir.Value);
        var fungusCost = sw.ElapsedMilliseconds - dataCost;

        sw.Stop();
        Main.LogInfo(string.Format("所有数据导出完毕，总耗时\t {0} s".I18NTodo(), sw.ElapsedMilliseconds / 1000f));
        Main.LogIndent += 1;
        Main.LogInfo(string.Format("Data导出耗时\t：{0} s".I18NTodo(), dataCost / 1000f));
        Main.LogInfo(string.Format("Fungus导出耗时\t：{0} s".I18NTodo(), fungusCost / 1000f));
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
            OnModLoadComplete();
            OnModSettingChanged();
        }
        sw.Stop();
        Main.LogInfo(string.Format("ModManager.ReloadComplete".I18N(), sw.ElapsedMilliseconds / 1000f));
    }
    
    public static async UniTask ReloadAllModAsync()
    {
        Main.LogInfo($"ModManager.StartReloadMod".I18N());
        await UniTask.SwitchToThreadPool();
        var sw = Stopwatch.StartNew();
        {
            OnModReload();
            RestoreBaseData();
            RestoreNextData();
            OnModLoadStart();
            LoadAllMod();
            InitJSONClassData();
            await UniTask.SwitchToMainThread();
            SceneManager.LoadScene("MainMenu");
            OnModLoadComplete();
            OnModSettingChanged();
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

    public static void CheckModLoadState()
    {
        bool hasError = false;
        var errorInfoSb = new StringBuilder();
        
        if (!_buildCacheSuccess)
        {
            errorInfoSb.AppendLine("Mod缓存构建失败！请检查Mod数据内容！");
            hasError = true;
        }
        
        var loadErrorMods = modGroups.SelectMany(g => g.ModConfigs).Where(c => c.Exception != null).ToArray();
        if (loadErrorMods.Length > 0)
        {
            errorInfoSb.AppendLine($"Mod加载出错！以下Mod加载出现异常，具体信息请查看Mod面板：\n" + 
                                   string.Join("\n", loadErrorMods.Select(mod => $"{Path.GetFileNameWithoutExtension(mod.Path)}({mod.Name ?? "Mod.Unknown".I18N() })"))
                                   );
            hasError = true;
        }

        if (hasError)
        {
            WindowConfirmDialog.CreateDialog("提示".I18NTodo(),errorInfoSb.ToString(), false);
        }
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
        Main.LogInfo($"===================" + "ModManager.LoadingModData".I18N() + "=====================");
        jsonData jsonInstance = jsonData.instance;

        ReloadModMeta(true, true);
        
        // 加载Mod数据
        foreach (var modGroup in modGroups)
        {
            foreach (var modConfig in modGroup.ModConfigs)
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
                    Main.LogError(string.Format("ModManager.LoadFail".I18N(),modGroup.GroupKey,modConfig.Path));
                    Main.LogError(e);
                    modConfig.Exception = e;
                }
            }
        }

        _buildCacheSuccess = false;
        try
        {
            // 重建Buff缓存
            jsonInstance.Buff.Clear();
            jsonInstance.InitBuff();
            _buildCacheSuccess = true;
        }
        catch (Exception e)
        {
            Main.LogError(e);
        }

        // 检查数据
        if (!CheckData.Check())
        {
            Main.LogError(CheckData.log);
        }
    }
        
    /// <summary>
    /// 重载mod元数据
    /// </summary>
    /// <param name="resetModState">是否重置Mod状态</param>
    public static void ReloadModMeta(bool resetModState,bool showLog = false)
    {
        if (resetModState)
        {
            modGroups.Clear();
        }
        
        DirectoryInfo testModPath = new DirectoryInfo(Main.PathLocalModsDir.Value);
        // 加载工坊模组
        foreach (var dir in WorkshopTool.GetAllModDirectory())
        {
            string workshopID = dir.Name;
            bool disable = WorkshopTool.CheckModIsDisable(workshopID);
            if (disable)
            {
                continue;
            }

            if (resetModState)
            {
                if (Directory.Exists(dir + @"\plugins\Next"))
                {
                    var modConfigGroup = new ModGroup(dir, ModType.Workshop);
                    modGroups.Add(modConfigGroup);
                }
            }
            else
            {
                if (Directory.Exists(dir + @"\plugins\Next"))
                {
                    var modConfigGroup = modGroups.Find(x => x.ModDir.FullName == dir.FullName);
                    if (modConfigGroup == null)
                    {
                        modConfigGroup = new ModGroup(dir, ModType.Workshop);
                        modGroups.Add(modConfigGroup);
                    }
                }
                else
                {
                    var modConfigGroup = modGroups.Find(x => x.ModDir.FullName == dir.FullName);
                    if (modConfigGroup != null)
                    {
                        modGroups.Remove(modConfigGroup);
                    }
                }
            }
        }
        
        // 加载本地模组
        foreach (var dir in testModPath.GetDirectories())
        {
            if (resetModState)
            {
                if (Directory.Exists(dir + @"\plugins\Next"))
                {
                    var modConfigGroup = new ModGroup(dir, ModType.Local);
                    modGroups.Add(modConfigGroup);
                }
            }
            else
            {
                if (Directory.Exists(dir + @"\plugins\Next"))
                {
                    var modConfigGroup = modGroups.Find(x => x.ModDir.FullName == dir.FullName);
                    if (modConfigGroup == null)
                    {
                        modConfigGroup = new ModGroup(dir, ModType.Local);
                        modGroups.Add(modConfigGroup);
                    }
                }
                else
                {
                    var modConfigGroup = modGroups.Find(x => x.ModDir.FullName == dir.FullName);
                    if (modConfigGroup != null)
                    {
                        modGroups.Remove(modConfigGroup);
                    }
                }
            }
        }
        
        // 加载元数据
        foreach (var modGroup in modGroups)
        {
            modGroup.Init(resetModState, showLog);
            foreach (var modConfig in modGroup.ModConfigs)
            {
                InitSettingData(modConfig);
            }
        }

        // 排序
        modGroups = SortModGroup(modGroups).ToList();
        ApplyModSetting(true);
        if(resetModState)
            SaveSetting();
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

    /// <summary>
    /// 加载Mod配置
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="showLog"></param>
    /// <returns></returns>
    public static ModConfig LoadModConfig(string dir,bool showLog)
    {
        var modConfig = GetModConfig(dir);
        if (showLog)
        {
            Main.LogInfo(string.Format("ModManager.LoadMod".I18N(), dir));
        }
        return modConfig;
    }
    
    /// <summary>
    /// Mod组排序
    /// </summary>
    /// <param name="modEnumerable"></param>
    /// <returns></returns>
    public static IEnumerable<ModGroup> SortModGroup(IEnumerable<ModGroup> modEnumerable)
    {
        var mods = modEnumerable.ToArray();
        var nextModSetting = Main.I.NextModSetting;

        var modSortList = mods
            .Select(modGroup => nextModSetting.GetOrCreateModGroupSetting(modGroup))
            .OrderBy(modSetting => modSetting.priority)
            .ThenBy(modSetting => modSetting.BindGroup.GroupKey);

        return modSortList.Select(data => data.BindGroup);
    }
    
    /// <summary>
    /// Mod排序
    /// </summary>
    /// <param name="modEnumerable"></param>
    /// <returns></returns>
    public static IEnumerable<ModConfig> SortMod(IEnumerable<ModConfig> modEnumerable)
    {
        var mods = modEnumerable.ToArray();
        var nextModSetting = Main.I.NextModSetting;

        var modSortList = mods
            .Select(modConfig => nextModSetting.GetOrCreateModSetting(modConfig))
            .OrderBy(modSetting => modSetting.priority)
            .ThenBy(modSetting => modSetting.BindMod.SettingKey);

        return modSortList.Select(data => data.BindMod);
    }

    /// <summary>
    /// 应用并重设Mod排序优先级
    /// </summary>
    /// <param name="applySubMod"></param>
    public static void ApplyModSetting(bool applySubMod)
    {
        var groupIndex = 0;
        var nextModSetting = Main.I.NextModSetting;
        foreach (var group in modGroups)
        {
            var setting = nextModSetting.GetOrCreateModGroupSetting(group);
            setting.priority = groupIndex++;
            
            if(applySubMod)
                group.ApplyModSetting();
        }
    }

    private static void LoadModData(ModConfig modConfig)
    {
        Main.LogInfo($"===================" + "ModManager.StartLoadMod".I18N() + "=====================");
        // 获取版本地址
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
                        modConfig.JsonPathCache.Add(fieldInfo.Name, filePath);
                        PatchJsonObject(fieldInfo, filePath, jsonObject);
                    }
                    else if (value is JObject jObject)
                    {
                        string filePath = Utility.CombinePaths(modDataDir, $"{fieldInfo.Name}.json");
                        modConfig.JsonPathCache.Add(fieldInfo.Name, filePath);
                        PatchJObject(fieldInfo, filePath, jObject);
                    }
                    else if (value is jsonData.YSDictionary<string, JSONObject> dicData)
                    {
                        string dirPathForData = Utility.CombinePaths(modDataDir, fieldInfo.Name);
                        JSONObject toJsonObject =
                            typeof(jsonData).GetField($"_{fieldInfo.Name}").GetValue(jsonInstance) as JSONObject;
                        modConfig.JsonPathCache.Add(fieldInfo.Name, dirPathForData);
                        PatchStrDicData(fieldInfo, dirPathForData, dicData, toJsonObject);
                    }
                    // 功能函数配置数据
                    else if (value is JSONObject[] jsonObjects)
                    {
                        string dirPathForData = Utility.CombinePaths(modDataDir, fieldInfo.Name);
                        modConfig.JsonPathCache.Add(fieldInfo.Name, dirPathForData);
                        PatchJsonObjectArray(fieldInfo, dirPathForData, jsonObjects);
                    }
                }
                catch (Exception e)
                {
                    throw new ModLoadException(string.Format("加载 Data【{0}】数据失败".I18NTodo(), fieldInfo.Name), e);
                }
            }
            
            // 加载AI数据
            {
                var dirPathForData = Utility.CombinePaths(modDataDir, "AIJsonDate");
                PatchIntDicData(dirPathForData, jsonInstance.AIJsonDate);
            }
            
            // 加载副本数据
            {
                var dirPathForData = Utility.CombinePaths(modDataDir, "FuBenJsonData");
                PatchFubenDicData(dirPathForData, jsonInstance.FuBenJsonData);
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
        catch (Exception e)
        {
            Main.LogWarning($"ModManager.ModConfigLoadFail".I18N());
            Main.LogError(e);
        }

        modConfig = modConfig ?? new ModConfig();
        modConfig.Path = dir;
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

    public static void PatchJObject(FieldInfo fieldInfo, string filePath, JObject jObject)
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

    public static void PatchStrDicData(FieldInfo fieldInfo,string dirPathForData, 
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
    
    private static void PatchIntDicData(string dirPathForData, 
        Dictionary<int,JSONObject> dictionary)
    {
        if (!Directory.Exists(dirPathForData))
            return;
        foreach (var filePath in Directory.GetFiles(dirPathForData))
        {
            try
            {
                var curData = LoadJSONObject(filePath);
                var key = Path.GetFileNameWithoutExtension(filePath);

                if (!int.TryParse(key, out var intKey))
                {
                    throw new ModLoadException($"不合法的文件ID {filePath} ，文件名必须为数字");
                }
                
                if (dictionary.TryGetValue(intKey, out var tagData))
                {
                    foreach (var fieldKey in curData.keys)
                    {
                        tagData.TryAddOrReplace(fieldKey,curData.GetField(fieldKey));
                    }
                }
                else
                {
                    dictionary[intKey] = curData;
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
    
    private static void PatchFubenDicData(string dirPathForData, Dictionary<int,List<JSONObject>> fuBenJsonData)
    {
        if (!Directory.Exists(dirPathForData))
            return;
        
        foreach (var dirPath in Directory.GetDirectories(dirPathForData))
        {
            var key = Path.GetFileNameWithoutExtension(dirPath);

            if (!int.TryParse(key, out var intKey))
            {
                throw new ModLoadException($"不合法的文件夹ID {dirPath} ，文件名必须为数字");
            }
            
            if(!fuBenJsonData.TryGetValue(intKey,out var list))
            {
                list = new List<JSONObject>();
                fuBenJsonData[intKey] = list;
                for (int i = 0; i < 4; i++)
                {
                    list.Add(new JSONObject());
                }
            }
            
            string filePath = Utility.CombinePaths(dirPathForData, $"RandomMap.json");
            PatchFubenJsonData(list[0], filePath);
            filePath = Utility.CombinePaths(dirPathForData, $"ShiJian.json");
            PatchFubenJsonData(list[1], filePath);
            filePath = Utility.CombinePaths(dirPathForData, $"RandomMap.json");
            PatchFubenJsonData(list[2], filePath);
            filePath = Utility.CombinePaths(dirPathForData, $"RandomMap.json");
            PatchFubenJsonData(list[3], filePath);
        }
    }

    private static void PatchFubenJsonData(JSONObject jsonData, string filePath)
    {
        try
        {
            var curData = LoadJSONObject(filePath);
            foreach (var fieldKey in curData.keys)
            {
                jsonData.TryAddOrReplace(fieldKey,curData.GetField(fieldKey));
            }
        }
        catch (Exception e)
        {
            throw new ModLoadException($"文件 {filePath} 解析失败", e);
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
                throw new ModLoadException(string.Format("DialogEvent {0} 加载失败。".I18NTodo(), filePath), e);
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
                throw new ModLoadException(string.Format("DialogTrigger {0} 加载失败。".I18NTodo(), filePath), e);
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
                throw new ModLoadException(string.Format("StaticFace {0} 转换失败。".I18NTodo(), filePath), e);
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
                throw new ModLoadException(string.Format("FPatch {0} 加载失败。".I18NTodo(), filePath), e);
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
                throw new ModLoadException(string.Format("Lua {0} 加载失败。".I18NTodo(), luaPath), e);
            }
        });
    }
    
    private static void InitSettingData(ModConfig modConfig)
    {
        foreach (var settingDefinition in modConfig.Settings)
        {
            if (settingDefinition is ModSettingDefinition_Custom customDefinition)
            {
                var customSetting = GetCustomSetting(customDefinition.CustomType);
                if(customSetting == null)
                    throw new ModLoadException(string.Format("自定义Mod设置类型：{0}不存在".I18NTodo(), customDefinition.CustomType));
                customSetting.OnInit(customDefinition);
            }
            else
            {
                settingDefinition.OnInit();
            }
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

    public static void ModGroupMoveUp(ModGroup modGroup)
    {
        var index = modGroups.IndexOf(modGroup);
        if (index == 0)
            return;
        ModGroupSetIndex(modGroup, index - 1);
    }
    
    public static void ModGroupMoveDown(ModGroup modGroup)
    {
        var index = modGroups.IndexOf(modGroup);
        if (index == modGroups.Count - 1)
            return;
        ModGroupSetIndex(modGroup, index + 1);
    }

    public static void ModGroupSetIndex(ModGroup group,int index)
    {
        var oldIndex = modGroups.IndexOf(group);
        modGroups.RemoveAt(oldIndex);
        modGroups.Insert(index, group);
        ApplyModSetting(false);
    }

    public static void ModSetEnable(ModConfig modConfig,bool enable)
    {
        Main.I.NextModSetting.GetOrCreateModSetting(modConfig).enable = enable;
    }
        
    public static bool ModGetEnable(ModConfig modConfig)
    {
        return Main.I.NextModSetting.GetOrCreateModSetting(modConfig).enable;
    }
    
    public static void ModSetEnableAll(bool b)
    {
        var nextModSetting = Main.I.NextModSetting;
        foreach (var modGroup in modGroups)
        {
            foreach (var modConfig in modGroup.ModConfigs)
            {
                nextModSetting.GetOrCreateModSetting(modConfig).enable = b;
            }
        }
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
            throw new ModLoadException(string.Format("文件 {0} 加载失败。".I18NTodo(), filePath), e);
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
            throw new ModLoadException(string.Format("文件 {0} 加载失败。".I18NTodo(), filePath), e);
        }
    }
    
    public static WorkShopItem ReadConfig(string path)
    {
        WorkShopItem result = new WorkShopItem();
        var configPath = path + "/Mod.bin";
        if (File.Exists(configPath))
        {
            try
            {
                FileStream fileStream = new FileStream(configPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                result = (WorkShopItem)new BinaryFormatter().Deserialize(fileStream);
                fileStream.Close();
            }
            catch (Exception message)
            {
                Main.LogError(message);
                Main.LogError("读取配置文件失败".I18NTodo());
            }
        }
        // result.SteamID = SteamUser.GetSteamID().m_SteamID;
        // result.ModPath = path;
        return result;
    }
        
    public static void WriteConfig(string path, WorkShopItem item)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);
        new BinaryFormatter().Serialize(fileStream, item);
        fileStream.Close();
    }
    
    public static void RegisterCustomSetting(string customType, ICustomSetting drawer)
    {
        _customSetting[customType] = drawer;
    }
    
    public static ICustomSetting GetCustomSetting(string customType)
    {
        if (_customSetting.TryGetValue(customType, out var drawer))
        {
            return drawer;
        }
        
        return null;
    }
    public static bool TryGetModSetting(string key, out bool value)
    {
        if(TryGetModSetting(key,out bool? config)){
            value = config ?? false ;
            return true;
        }
        value = false;
        return false;
    }
    
    public static void SetModSetting(string key, bool value)
    {
        var setting = Main.I.NextModSetting;
        setting.BoolGroup.Set(key, value);
    }
    
    public static bool TryGetModSetting(string key, out bool? value)
    {
        var setting = Main.I.NextModSetting;
        if(setting.BoolGroup.Has(key))
        {
            value = setting.BoolGroup.Get(key);
            return true;
        }
        value = null;
        return false;
    }
    
    public static void SetModSetting(string key, bool? value)
    {
        var setting = Main.I.NextModSetting;
        setting.BoolGroup.Set(key, value);
    }

    public static bool TryGetModSetting(string key, out long value)
    {
        var setting = Main.I.NextModSetting;
        if(setting.LongIntegerGroup.Has(key))
        {
            value = setting.LongIntegerGroup.Get(key);
            return true;
        }
        value = 0L;
        return false;
    }
    
    public static void SetModSetting(string key, long value)
    {
        var setting = Main.I.NextModSetting;
        setting.LongIntegerGroup.Set(key, value);
    }

    public static bool TryGetModSetting(string key, out double value)
    {
        var setting = Main.I.NextModSetting;
        if(setting.DoubleFloatGroup.Has(key))
        {
            value = setting.DoubleFloatGroup.Get(key);
            return true;
        }
        value = 0.0;
        return false;
    }
    
    public static void SetModSetting(string key, double value)
    {
        var setting = Main.I.NextModSetting;
        setting.DoubleFloatGroup.Set(key, value);
    }
    
    public static bool TryGetModSetting(string key, out string value)
    {
        var setting = Main.I.NextModSetting;
        if(setting.StringGroup.Has(key))
        {
            value = setting.StringGroup.Get(key);
            return true;
        }
        value = null;
        return false;
    }
    
    public static void SetModSetting(string key, string value)
    {
        var setting = Main.I.NextModSetting;
        setting.StringGroup.Set(key, value);
    }

    public static void SaveSetting()
    {
        Main.I.SaveModSetting();
        OnModSettingChanged();
    }

    public static void LoadSetting()
    {
        Main.I.LoadModSetting();
        OnModSettingChanged();
    }
    
    #endregion

    #region 私有方法

    #endregion
}