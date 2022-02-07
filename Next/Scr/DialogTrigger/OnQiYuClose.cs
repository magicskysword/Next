using HarmonyLib;

namespace SkySwordKill.Next.DialogTrigger
{
    [HarmonyPatch(typeof(QiYu.QiYuUIMag),"Close")]
    public class OnQiYuClose
    {
        [HarmonyPostfix]
        public static void Postfix(QiYu.QiYuUIMag __instance)
        {
            if (__instance.EventId == 0)
                return;
            Main.LogInfo($"关闭奇遇 ID : [{__instance.EventId}] 已选选项 : [{OnQiYuShow.lastOption}]");
            var env = new DialogEnvironment()
            {
                qiyuID = __instance.EventId,
                qiyuOption = OnQiYuShow.lastOption
            };
            DialogAnalysis.TryTrigger(new []{"奇遇关闭","AdventureClose"}, env);
        }
    }
}