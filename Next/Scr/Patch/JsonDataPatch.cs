using System.Diagnostics;
using HarmonyLib;
using SkySwordKill.Next.Mod;

namespace SkySwordKill.Next.Patch
{
    [HarmonyPatch(typeof(YSJSONHelper), "InitJSONClassData")]
    public class JsonDataPatch
    {
        [HarmonyPrefix]
        public static void FixMethod()
        {
            var watcher = Stopwatch.StartNew();
            ModManager.CloneMainData();
            watcher.Stop();
            Main.LogInfo($"储存数据耗时：{watcher.ElapsedMilliseconds / 1000f} s");
            
            ModManager.FirstLoadAllMod();
        }
    }
}