using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("LearnSkill")]
    public class LearnSkill: IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int id = command.GetInt(0);
            
            env.player.addHasSkillList(id);
            
            MessageMag.Instance.Send(MessageName.MSG_PLAYER_USE_ITEM, null);
            callback?.Invoke();
        }
    }
}