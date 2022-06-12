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
            ModManager.FirstLoadAllMod();
        }
    }
}