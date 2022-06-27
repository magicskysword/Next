using System.Reflection;
using HarmonyLib;

namespace SkySwordKill.Next.DialogTrigger
{
    [HarmonyPatch(typeof(UINPCJiaoHuPop),"OnJiaoTanBtnClick")]
    public class OnTalk
    {
        public static bool Prefix(UINPCJiaoHuPop __instance)
        {
            UINPCData npc = typeof(UINPCJiaoHuPop).GetField("npc",
                BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(__instance) as UINPCData;
            Main.LogInfo($"交谈对象ID : [{npc.ID}] 绑定ID : [{npc.ZhongYaoNPCID}]");
            var env = new DialogEnvironment()
            {
                bindNpc = npc,
                roleBindID = npc.ZhongYaoNPCID,
                roleID = npc.ID,
                roleName = npc.Name
            };
            if (DialogAnalysis.TryTrigger(new []{"交谈","Talk"}, env))
            {
                UINPCJiaoHu.Inst.HideJiaoHuPop();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}