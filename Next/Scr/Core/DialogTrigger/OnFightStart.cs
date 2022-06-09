using SkySwordKill.Next.DialogEvent;

namespace SkySwordKill.Next.DialogTrigger
{
    public class OnFightStart
    {
        public static void Trigger()
        {
            var env = new DialogEnvironment()
            {
                fightTags = StartFight.FightTags,
                roleID = Tools.instance.MonstarID
            };
            DialogAnalysis.TryTrigger(new []{"战斗开始","FightStart"}, env);
        }
    }
}