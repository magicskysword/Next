using HarmonyLib;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogTrigger
{
    public class OnEnterGame
    {
        public static bool IsEnterGame { get; set; } = false;

        public static void Trigger()
        {
            Main.LogInfo($"进入游戏触发");
            var env = new DialogEnvironment();
            DialogAnalysis.TryTrigger(new[] { "进入游戏", "EnterGame" }, env, true);
        }
    }

    [HarmonyPatch(typeof(CreateNewPlayerFactory),"createPlayer")]
    public class OnEnterGameAfterCreatePlayer
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            OnEnterGame.IsEnterGame = true;
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
            Main.LogInfo($"读取存档 - 游戏内");
        }
    }
    
    [HarmonyPatch(typeof(MainUIMag),"addAvatar")]
    public class OnEnterGameAfterLoadMain
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            OnEnterGame.IsEnterGame = true;
            Main.LogInfo($"读取存档 - 主界面");
        }
    }

    [HarmonyPatch(typeof(GUIPackage.UI_Manager), "Update")]
    public class AfterBattleDialogPatch
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            if (!Tools.instance.isNeedSetTalk)
                return;
            if (OnEnterGame.IsEnterGame)
            {
                OnEnterGame.IsEnterGame = false;
                OnEnterGame.Trigger();
            }
        }
    }
}