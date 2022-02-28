using System.Reflection;
using HarmonyLib;
using JSONClass;
using UnityEngine;

namespace SkySwordKill.Next.Patch
{
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
                var staticSkillIDByKey = GetStaticSkillIconByKey(__instance.skill_ID);
                var path = $"StaticSkill Icon/{staticSkillIDByKey}";
                if (Main.Instance.resourcesManager.TryGetAsset($"Assets/{path}.png", asset =>
                {
                    if (asset is Texture2D texture)
                    {
                        __instance.skill_Icon = texture;
                        __instance.skillIconSprite = Main.Instance.resourcesManager.GetSpriteCache(texture);
                    }
                }))
                {
                    Main.LogInfo($"功法 [{__instance.skill_ID}] 图标加载成功");
                }
                else
                {
                    Texture2D exists = Resources.Load<Texture2D>(path);
                    if (exists)
                    {
                        __instance.skill_Icon = exists;
                    }
                    else
                    {
                        __instance.skill_Icon = Resources.Load<Texture2D>("StaticSkill Icon/0");
                    }
                    Sprite exists2 = Resources.Load<Sprite>(path);
                    if (exists2)
                    {
                        __instance.skillIconSprite = exists2;
                    }
                    else
                    {
                        __instance.skillIconSprite = Resources.Load<Sprite>("StaticSkill Icon/0");
                    }
                }
            }
            else
            {
                // 神通图标
                var path = $"Skill Icon/{GetSkillIconByKey(__instance.skill_ID)}";
                if (Main.Instance.resourcesManager.TryGetAsset($"Assets/{path}.png", asset =>
                {
                    if (asset is Texture2D texture)
                    {
                        __instance.skill_Icon = texture;
                        __instance.skillIconSprite = Main.Instance.resourcesManager.GetSpriteCache(texture);
                    }
                }))
                {
                    Main.LogInfo($"技能 [{__instance.skill_ID}] 图标加载成功");
                }
                else
                {
                    Texture2D exists = Resources.Load<Texture2D>(path);
                    if (exists)
                    {
                        __instance.skill_Icon = exists;
                    }
                    else
                    {
                        __instance.skill_Icon = Resources.Load<Texture2D>("Skill Icon/0");
                    }
                    Sprite exists2 = Resources.Load<Sprite>(path);
                    if (exists2)
                    {
                        __instance.skillIconSprite = exists2;
                    }
                    else
                    {
                        __instance.skillIconSprite = Resources.Load<Sprite>("Skill Icon/0");
                    }
                };
            }
        }
    }
}