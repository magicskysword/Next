using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Fungus;
using Newtonsoft.Json;
using SkySwordKill.Next.FCanvas.PatchCommand;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkySwordKill.Next.FCanvas;

public static class FFlowchartTools
{
    private static Dictionary<string, Type> _fCommandBinderDic;

    public static Dictionary<string, Type> FCommandBinderDic
    {
        get
        {
            if (_fCommandBinderDic == null)
            {
                _fCommandBinderDic = new Dictionary<string, Type>();
                foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    var f = type.GetCustomAttribute<FCommandBinderAttribute>();
                    if (f?.Type?.FullName != null)
                    {
                        Main.LogInfo($"Bind FCommand {f.Type} {type}");
                        _fCommandBinderDic.Add(f.Type.FullName, type);
                    }
                }
            }

            return _fCommandBinderDic;
        }
    }
        
    private static Dictionary<string, Type> _pCommandBinderDic;

    public static Dictionary<string, Type> PCommandBinderDic
    {
        get
        {
            if (_pCommandBinderDic == null)
            {
                _pCommandBinderDic = new Dictionary<string, Type>();
                foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    var p = type.GetCustomAttribute<PCommandBinderAttribute>();
                    if (p?.BindType != null)
                    {
                        Main.LogInfo($"Bind PCommand {p.BindType} {type}");
                        _pCommandBinderDic.Add(p.BindType, type);
                    }
                }
            }

            return _pCommandBinderDic;
        }
    }

    public static FFlowchart ConvertToFFlowchart(this Flowchart flowchart)
    {
        var newFlowchart = new FFlowchart();
        newFlowchart.ReadFlowchart(flowchart);
        return newFlowchart;
    }

    public static void ExportAllFungusFlowchart(string outputPath)
    {
        var go = Resources.LoadAll<GameObject>("");
        var flowcharts = go
            .Select(g => g.GetComponentInChildren<Flowchart>());
        if(Directory.Exists(outputPath))
            Directory.Delete(outputPath, true);
        Directory.CreateDirectory(outputPath);
        foreach (var flowchart in flowcharts)
        {
            if(flowchart == null)
                continue;

            try
            {
                ExportFungusFlowchart(flowchart, outputPath);
            }
            catch (Exception e)
            {
                Main.LogError(e);
            }
        }
    }
    
    public static void ExportCurrentSceneFungusFlowchart(string outputPath)
    {
        var flowchartsInScene = new List<Flowchart>();
        foreach (var rootGameObject in Resources.FindObjectsOfTypeAll<Flowchart>())
        {
            rootGameObject.GetComponentsInChildren(true,  flowchartsInScene);
            foreach (var flowchart in flowchartsInScene)
            {
                try
                {
                    ExportFungusFlowchart(flowchart, outputPath);
                }
                catch (Exception e)
                {
                    Main.LogError(e);
                }
            }
        }
    }

    private static void ExportFungusFlowchart(Flowchart flowchart, string outputPath)
    {
        var fFlowchart = flowchart.ConvertToFFlowchart();
        var json = JsonConvert.SerializeObject(fFlowchart, Formatting.Indented);
        var pathName = $"{outputPath}/{fFlowchart.Name}.json";
                    
        File.WriteAllText(pathName ,json);
        Main.LogInfo($"Export Fungus {fFlowchart.Name} to {pathName}");
    }

    public static Dictionary<string, FFlowchart> ImportAllFFlowchart(string dir)
    {
        var fflowcharts = new Dictionary<string, FFlowchart>();
            
        if (!Directory.Exists(dir))
            return fflowcharts;

        foreach (var path in Directory.GetFiles(dir))
        {
            var f = ImportFFlowchart(path);
            if (f == null)
            {
                Main.LogWarning($"FFlowchart为空， Path : {path}");
                continue;
            }
            if(fflowcharts.ContainsKey(f.Name))
            {
                Main.LogWarning($"存在相同的FFlowchart，Name : {f.Name} Path : {path}");
            }
            fflowcharts[f.Name] = f;
        }

        return fflowcharts;
    }
        
    public static FFlowchart ImportFFlowchart(string path)
    {
        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<FFlowchart>(json);
    }

    public static FCommand CreateBindFCommand(this Type cmdType)
    {
        var fCmd = (FCommand)Activator.CreateInstance(cmdType.GetFCommandType());
        return fCmd;
    }

    public static Type GetFCommandType(this Type cmdType)
    {
        var fullName = cmdType.FullName ?? "";
        return FCommandBinderDic.TryGetValue(fullName, out var fCmdType) ? fCmdType : typeof(FCommand);
    }

    public static FCommand CreateFCommand(string cmdType)
    {
        if(FCommandBinderDic.ContainsKey(cmdType))
        {
            return (FCommand)Activator.CreateInstance(FCommandBinderDic[cmdType]);
        }
            
        return new FCommand();
    }

    public static PCommand GetPCommandType(GameObject go,string cmdType)
    {
        Type pCmdType = typeof(PCommand);
        if(PCommandBinderDic.ContainsKey(cmdType))
        {
            pCmdType = PCommandBinderDic[cmdType];
        }
            
        var pCmd = (PCommand)go.AddComponent(pCmdType);
        return pCmd;
    }
}