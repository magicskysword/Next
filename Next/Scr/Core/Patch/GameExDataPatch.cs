using HarmonyLib;

namespace SkySwordKill.Next.Patch
{
    public class GameExDataPatch
    {
        [HarmonyPatch(typeof(Tools),"saveGame")]
        public class SaveGamePatch
        {
            [HarmonyPostfix]
            public static void Postfix(Tools __instance,int id, int index, KBEngine.Avatar _avatar)
            {
                Main.LogInfo($"保存 AvatarNextData");
                DialogAnalysis.SaveAvatarNextData(id, index);
            }
        }
        
        [HarmonyPatch(typeof(CreateNewPlayerFactory),"createPlayer")]
        public class OnCreatePlayer
        {
            [HarmonyPrefix]
            public static void Prefix()
            {
                DialogAnalysis.ResetAvatarNextData();
            }
        }
        
        [HarmonyPatch(typeof(StartGame),"addAvatar")]
        public class OnLoadInStart
        {
            [HarmonyPostfix]
            public static void Postfix(int id, int index)
            {
                Main.LogInfo($"加载 AvatarNextData");
                DialogAnalysis.LoadAvatarNextData(id,index);
            }
        }
    
        [HarmonyPatch(typeof(MainUIMag),"addAvatar")]
        public class OnLoadInMain
        {
            [HarmonyPostfix]
            public static void Postfix(int id, int index)
            {
                Main.LogInfo($"加载 AvatarNextData");
                DialogAnalysis.LoadAvatarNextData(id,index);
            }
        }
    }
}