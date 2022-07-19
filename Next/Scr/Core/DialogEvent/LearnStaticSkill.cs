using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("LearnStaticSkill")]
    public class LearnStaticSkill: IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int id = command.GetInt(0);
            
            env.player.addHasStaticSkillList(id);
            
            MessageMag.Instance.Send(MessageName.MSG_PLAYER_USE_ITEM, null);
            callback?.Invoke();
        }
    }
}