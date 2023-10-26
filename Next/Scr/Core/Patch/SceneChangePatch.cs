using HarmonyLib;

namespace SkySwordKill.Next.Patch;

/// <summary>
/// 用于在场景切换时，自动销毁相关的UI，以及释放图片缓存
/// </summary>
public class SceneChangePatch
{
    [HarmonyPatch(typeof(Tools), "loadMapScenes")]
    public static void OnMapSceneLoad()
    {
        Main.FGUI.AutoCloseUIOnLoadScene();
        Main.Res.ReleaseCache();
    }
    
    [HarmonyPatch(typeof(Tools), "loadOtherScenes")]
    public static void OnOtherSceneLoad()
    {
        Main.FGUI.AutoCloseUIOnLoadScene();
        Main.Res.ReleaseCache();
    }
}