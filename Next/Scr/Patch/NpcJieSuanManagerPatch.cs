using HarmonyLib;

namespace SkySwordKill.Next.Patch
{
    [HarmonyPatch(typeof(NpcJieSuanManager),"InitCyData")]
    public class NpcJieSuanManagerPatch
    {
        [HarmonyPrefix]
        public static void FixMethod(NpcJieSuanManager __instance)
        {
            Main.LogInfo("重新Patch Npc数据");
            jsonData jsonInstance = jsonData.instance;
            var fieldInfo = typeof(jsonData).GetField("AvatarJsonData");
            foreach (var modConfig in ModManager.modConfigs)
            {
                if (modConfig.jsonPathCache.TryGetValue("AvatarJsonData",out var path))
                {
                    ModManager.PatchJsonObject(fieldInfo,path,jsonInstance.AvatarJsonData);
                }
            }
        }
    }
}