using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fungus;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SkySwordKill.Next.Patch
{
    [HarmonyPatch]
    public class FungusClonePatch
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            var methods = new List<MethodBase>();
            var normalTypes = typeof(Object)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(method => method.Name == "Instantiate" && !method.IsGenericMethod);
            methods.AddRange(normalTypes);
            var genericTypes = typeof(Object)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(method => method.Name == "Instantiate" && method.IsGenericMethod);
            foreach (var type in genericTypes)
            {
                methods.Add(type.MakeGenericMethod(typeof(Object)));
            }
            return methods;
        }

        [HarmonyPostfix]
        public static void Postfix(ref Object __result)
        {
            if (__result is GameObject go)
            {
                var flowcharts = go.GetComponentsInChildren<Flowchart>();
                if (flowcharts != null)
                {
                    foreach (var flowchart in flowcharts)
                    {
                        Main.FPatch.PatchFlowchart(flowchart);
                    }
                }
            }
        }
    }
}