using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Fungus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace SkySwordKill.Next.FungusTools
{
    public static class FungusTools
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


                    fFlowchart.Name = flowchart.GetParentName();
                    var json = JObject.FromObject(fFlowchart).ToString(Formatting.Indented);
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