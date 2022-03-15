using HarmonyLib;
using KBEngine;

namespace SkySwordKill.Next.DialogTrigger
{
    [HarmonyPatch(typeof(Avatar),"AddTime")]
    public class OnTimeChange
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            var env = new DialogEnvironment();
            DialogAnalysis.TryTrigger(new[] { "时间变化", "TimeChange" }, env, true);
        }
    }
}