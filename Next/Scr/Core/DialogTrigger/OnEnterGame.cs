using HarmonyLib;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogTrigger;

public class OnEnterGame
{
    public static bool IsEnterGame { get; set; } = false;
    public static bool IsFirstEnterGame { get; set; } = false;

    public static void TriggerFirstEnterGame()
    {
        Main.LogInfo($"首次进入游戏触发");
        var env = new DialogEnvironment();
        DialogAnalysis.TryTrigger(new[] { "首次进入游戏", "FirstEnterGame" }, env, true);
    }
        
    public static void TriggerEnterGame()
    {
        Main.LogInfo($"进入游戏触发");
        var env = new DialogEnvironment();
        DialogAnalysis.TryTrigger(new[] { "进入游戏", "EnterGame" }, env, true);
    }
}

[HarmonyPatch(typeof(YSNewSaveSystem))]
public class NewSaveSystemPatch
{
    [HarmonyPatch("LoadSave")]
    [HarmonyPostfix]
    public static void AfterLoad(int avatarIndex, int slot, int DFIndex)
    {
        OnEnterGame.IsEnterGame = true;
        Main.LogInfo($"读取存档");
    }
}

[HarmonyPatch(typeof(CreateNewPlayerFactory),"createPlayer")]
public class OnEnterGameAfterCreatePlayer
{
    [HarmonyPrefix]
    public static void Prefix()
    {
        OnEnterGame.IsEnterGame = true;
        OnEnterGame.IsFirstEnterGame = true;
        Main.LogInfo($"创建角色");
    }
}
    
[HarmonyPatch(typeof(StartGame),"addAvatar")]
public class OnEnterGameAfterLoad
{
    [HarmonyPrefix]
    public static void Prefix()
    {
        OnEnterGame.IsEnterGame = true;
        Main.LogInfo($"读取存档 - 旧版本 - 游戏内");
    }
}
    
[HarmonyPatch(typeof(MainUIMag),"addAvatar")]
public class OnEnterGameAfterLoadMain
{
    [HarmonyPrefix]
    public static void Prefix()
    {
        OnEnterGame.IsEnterGame = true;
        Main.LogInfo($"读取存档 - 旧版本 - 主界面");
    }
}

[HarmonyPatch(typeof(GUIPackage.UI_Manager), "Update")]
public class CheckEnterGame
{
    [HarmonyPrefix]
    public static void Prefix()
    {
        if (Tools.instance.isNeedSetTalk)
        {
            if (OnEnterGame.IsFirstEnterGame)
            {
                OnEnterGame.IsFirstEnterGame = false;
                OnEnterGame.TriggerFirstEnterGame();
            }
            if (OnEnterGame.IsEnterGame)
            {
                OnEnterGame.IsEnterGame = false;
                OnEnterGame.TriggerEnterGame();
            }
        }
    }
}