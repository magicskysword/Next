using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Fungus;
using Newtonsoft.Json;
using UnityEngine;

namespace SkySwordKill.Next.FCanvas
{
    public static class FFlowchartTools
    {
        private static Dictionary<Type, Type> _fCommandBinderDic;

        private static List<FPatch> FungusPatches;

        public static Dictionary<Type, Type> FCommandBinderDic
        {
            get
            {
                if (_fCommandBinderDic == null)
                {
                    _fCommandBinderDic = new Dictionary<Type, Type>();
                    foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                    {
                        var f = type.GetCustomAttribute<FCommandBinderAttribute>();
                        if (f != null)
                        {
                            Main.LogInfo($"Bind {f.Type} {type}");
                            _fCommandBinderDic.Add(f.Type, type);
                        }
                    }
                }

                return _fCommandBinderDic;
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
                    var fFlowchart = flowchart.ConvertToFFlowchart();
                    var json = JsonConvert.SerializeObject(fFlowchart, Formatting.Indented, new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                    var pathName = $"{outputPath}/{fFlowchart.Name}.json";
                    
                    File.WriteAllText(pathName ,json);
                    Main.LogInfo($"导出：{pathName}");
                }
                catch (Exception e)
                {
                    Main.LogError(e);
                }
            }
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
            return JsonConvert.DeserializeObject<FFlowchart>(json, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }

        public static FCommand CreateBindFCommand(this Type cmdType)
        {
            var fCmd = (FCommand)Activator.CreateInstance(cmdType.GetFCommandType());
            return fCmd;
        }

        public static Type GetFCommandType(this Type cmdType)
        {
            return FCommandBinderDic.TryGetValue(cmdType, out var fCmdType) ? fCmdType : typeof(FCommand);
        }
    }
}