using HarmonyLib;
using UnityEngine;

namespace SkySwordKill.Next.Patch
{
    /// <summary>
    /// 神通 - 技能图标Patch
    /// </summary>
    [HarmonyPatch(typeof(Bag.ActiveSkill),"GetIconSprite")]
    public class BagActiveSkillGetIconSprite
    {
        [HarmonyPrefix]
        public static bool LoadSprite(Bag.ActiveSkill __instance,ref Sprite __result)
        {
            __result = ResManager.inst.LoadSprite("Skill Icon/" + SkillIconPatch.GetSkillIconByKey(__instance.Id));
            return false;
        }
    }
}