using System.Diagnostics;
using HarmonyLib;
using SkySwordKill.Next.Mod;

namespace SkySwordKill.Next.Patch
{
    /// <summary>
    /// 游戏Mod注入Patch
    /// 重要Patch！
    /// Important Patch!
    /// </summary>
    [HarmonyPatch(typeof(YSJSONHelper), "InitJSONClassData")]
    public class JsonDataPatch
    {
        [HarmonyPrefix]
        public static void FixMethod()
        {
            var watcher = Stopwatch.StartNew();
            // 缓存游戏数据
            ModManager.CloneMainData();
            watcher.Stop();
            Main.LogInfo($"储存数据耗时：{watcher.ElapsedMilliseconds / 1000f} s");
            
            ModManager.FirstLoadAllMod();
        }
    }
}