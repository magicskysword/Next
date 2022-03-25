using HarmonyLib;
using SkySwordKill.Next.Extension;
using UnityEngine.Events;

namespace SkySwordKill.Next.DialogTrigger
{
    [HarmonyPatch(typeof(GUIPackage.item),"gongneng")]
    public class OnUseItem
    {
        [HarmonyPrefix]
        public static bool Prefix(GUIPackage.item __instance,UnityAction Next, bool isTuPo,ref bool __state)
        {
            Main.LogInfo($"[{"Trigger".I18N()}] {"Trigger.UseItem".I18N()} {__instance.itemID} [{__instance.itemName}]");
            var env = new DialogEnvironment()
            {
                itemID = __instance.itemID
            };
            if (DialogAnalysis.TryTrigger(new[] { "使用物品", "UseItem" }, env))
            {
                __state = true;
                return false;
            }
            
            return true;
        }
        
        [HarmonyPostfix]
        public static void Postfix(GUIPackage.item __instance,UnityAction Next, bool isTuPo,bool __state)
        {
            if(__state)
                return;
            
            Main.LogInfo($"[{"Trigger".I18N()}] {"Trigger.AfterUseItem".I18N()} {__instance.itemID} [{__instance.itemName}]");
            var env = new DialogEnvironment()
            {
                itemID = __instance.itemID
            };
            DialogAnalysis.TryTrigger(new[] { "使用物品后", "AfterUseItem" }, env);
        }
    }
}