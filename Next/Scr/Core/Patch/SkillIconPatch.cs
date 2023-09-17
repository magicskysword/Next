using System.Reflection;
using HarmonyLib;
using JSONClass;
using UnityEngine;

namespace SkySwordKill.Next.Patch;

/// <summary>
/// 技能图标Patch
/// </summary>
[HarmonyPatch(typeof(GUIPackage.Skill), "InitImage")]
public class SkillIconPatch
{
    public static int GetStaticSkillIconByKey(int key)
    {
        if (key < 0)
            return -1;
        else
        {
            var staticSkillJsonData = StaticSkillJsonData.DataDict[key];
            return staticSkillJsonData.icon > 0 ? staticSkillJsonData.icon : staticSkillJsonData.Skill_ID;
        }
    }

    public static int GetSkillIconByKey(int key)
    {
        if (key < 0)
            return -1;
        else
        {
            var skillJsonData = _skillJsonData.DataDict[key];
            return skillJsonData.icon > 0 ? skillJsonData.icon : skillJsonData.Skill_ID;
        }
    }

    public static void Prefix(GUIPackage.Skill __instance, out bool __state)
    {
        __state = !(bool)typeof(GUIPackage.Skill)
            .GetField("initedImage", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(__instance);
    }

    [HarmonyPostfix]
    public static void Postfix(GUIPackage.Skill __instance,bool __state)
    {
        if(!__state)
            return;
            
        if (__instance.IsStaticSkill)
        {
            // 功法图标
            var path = $"StaticSkill Icon/{GetStaticSkillIconByKey(__instance.skill_ID)}";
            Texture2D texture2D = ResManager.inst.LoadTexture2D(path);
            __instance.skill_Icon = texture2D == null ? ResManager.inst.LoadTexture2D("StaticSkill Icon/0") : texture2D;
            Sprite sprite = ResManager.inst.LoadSprite(path);
            __instance.skillIconSprite = sprite == null ? ResManager.inst.LoadSprite("StaticSkill Icon/0") : sprite;
        }
        else
        {
            // 神通图标
            var path = $"Skill Icon/{GetSkillIconByKey(__instance.skill_ID)}";
            Texture2D texture2D = ResManager.inst.LoadTexture2D(path);
            __instance.skill_Icon = texture2D == null ? ResManager.inst.LoadTexture2D("Skill Icon/0") : texture2D;
            Sprite sprite = ResManager.inst.LoadSprite(path);
            __instance.skillIconSprite = sprite == null ? ResManager.inst.LoadSprite("Skill Icon/0") : sprite;
        }
    }
}