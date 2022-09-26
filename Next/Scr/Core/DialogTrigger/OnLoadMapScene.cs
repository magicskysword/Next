using HarmonyLib;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogTrigger;

[HarmonyPatch(typeof(Tools), "loadMapScenes")]
public class OnLoadMapScene
{
    [HarmonyPostfix,]
    public static void Postfix(string name)
    {
        var env = new DialogEnvironment()
        {
            mapScene = name
        };
        DialogAnalysis.TryTrigger(new string[]
        {
            "进入场景",
            "EnterMapScene"
        }, env, true);
    }
}