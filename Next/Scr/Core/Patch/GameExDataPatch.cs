using HarmonyLib;
using KBEngine;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.Patch;

public class GameExDataPatch
{
    [HarmonyPatch(typeof(YSNewSaveSystem))]
    public class NewSaveSystemPatch
    {
        [HarmonyPatch("LoadSave")]
        [HarmonyPostfix]
        public static void AfterLoad(int avatarIndex, int slot, int DFIndex)
        {
            Main.LogInfo($"NextData 读取存档数据");
            DialogAnalysis.LoadAvatarNextData(avatarIndex, slot);
            DialogAnalysis.OnEnterWorld();
        }
        
        [HarmonyPatch("SaveGame")]
        [HarmonyPostfix]
        public static void AfterSave(int avatarIndex, int slot, Avatar _avatar, bool ignoreSlot0Time)
        {
            Main.LogInfo($"NextData 保存存档数据");
            DialogAnalysis.SaveAvatarNextData(avatarIndex, slot);
        }
    }
        
    [HarmonyPatch(typeof(Tools),"saveGame")]
    public class OldSaveGamePatch
    {
        [HarmonyPostfix]
        public static void Postfix(Tools __instance,int id, int index, KBEngine.Avatar _avatar)
        {
            if (jsonData.instance.saveState == 1)
                return;
            if (!NpcJieSuanManager.inst.isCanJieSuan)
                return;
            if (FpUIMag.inst != null || TpUIMag.inst != null || UINPCJiaoHu.Inst.NowIsJiaoHu2 || SetFaceUI.Inst != null)
                return;
            Main.LogInfo($"NextData 保存旧版存档数据");
            DialogAnalysis.SaveAvatarNextDataOld(id, index);
        }
    }
        
    [HarmonyPatch(typeof(CreateNewPlayerFactory),"createPlayer")]
    public class OldCreatePlayer
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            Main.LogInfo($"NextData 创建新角色数据");
            DialogAnalysis.ResetAvatarNextData();
            DialogAnalysis.OnEnterWorld();
        }
    }
        
    [HarmonyPatch(typeof(StartGame),"addAvatar")]
    public class OldLoadInStart
    {
        [HarmonyPostfix]
        public static void Postfix(int id, int index)
        {
            Main.LogInfo($"NextData 读取旧版存档数据");
            DialogAnalysis.LoadAvatarNextDataOld(id,index);
            DialogAnalysis.OnEnterWorld();
        }
    }
    
    [HarmonyPatch(typeof(MainUIMag),"addAvatar")]
    public class OldLoadInMain
    {
        [HarmonyPostfix]
        public static void Postfix(int id, int index)
        {
            Main.LogInfo($"NextData 读取旧版存档数据");
            DialogAnalysis.LoadAvatarNextDataOld(id,index);
            DialogAnalysis.OnEnterWorld();
        }
    }
}