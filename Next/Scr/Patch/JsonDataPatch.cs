using HarmonyLib;

namespace SkySwordKill.Next.Patch
{
    [HarmonyPatch(typeof(YSJSONHelper), "InitJSONClassData")]
    public class JsonDataPatch
    {
        [HarmonyPrefix]
        public static void FixMethod()
        {
            Main.Instance.PatchJson();
        }
    }
}