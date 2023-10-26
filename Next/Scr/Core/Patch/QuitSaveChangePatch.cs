using HarmonyLib;
using Tab;

namespace SkySwordKill.Next.Patch;

/// <summary>
/// 用于在读取存档、返回主菜单时，自动销毁相关的UI
/// </summary>
public class QuitSaveChangePatch
{
    [HarmonyPatch(typeof(YSNewSaveSystem) ,"LoadSave")]
    [HarmonyPostfix]
    public static void OnLoadSave()
    {
        Main.FGUI.AutoCloseUIOnSaveQuit();
    }
    
    [HarmonyPatch(typeof(TabSystemPanel) ,"ReturnTittle")]
    [HarmonyPostfix]
    public static void OnReturnTitle()
    {
        Main.FGUI.AutoCloseUIOnSaveQuit();
    }
}