using HarmonyLib;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogTrigger
{
    [HarmonyPatch(typeof(QiYu.QiYuUIMag),"OptionAction")]
    public class OnQiYuOption
    {
        [HarmonyPostfix]
        public static void Postfix(QiYu.QiYuUIMag __instance,int optionId)
        {
            Main.LogInfo($"奇遇ID : [{__instance.EventId}] 选项ID : [{optionId}]");
            var env = new DialogEnvironment()
            {
                qiyuID = __instance.EventId,
                qiyuOption = optionId
            };
            if (DialogAnalysis.TryTrigger(new []{"奇遇选项","AdventureOption"}, env))
            {
                __instance.Close();
            }
            else
            {
                OnQiYuShow.lastOption = optionId;
            }
        }
    }
}