using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("LearnTrainSkill")]
    public class LearnTrainSkill: IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int id = command.GetInt(0);
            
            PlayerEx.StudyShuangXiuSkill(id);
            
            MessageMag.Instance.Send(MessageName.MSG_PLAYER_USE_ITEM, null);
            callback?.Invoke();
        }
    }
}