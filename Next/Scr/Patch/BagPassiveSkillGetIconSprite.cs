using HarmonyLib;
using UnityEngine;

namespace SkySwordKill.Next.Patch
{
    [HarmonyPatch(typeof(Bag.PassiveSkill),"GetIconSprite")]
    public class BagPassiveSkillGetIconSprite
    {
        [HarmonyPrefix]
        public static bool LoadSprite(Bag.PassiveSkill __instance,ref Sprite __result)
        {
            __result = ResManager.inst.LoadSprite("StaticSkill Icon/" + SkillIconPatch.GetStaticSkillIconByKey(__instance.Id));
            return false;
        }
    }
}