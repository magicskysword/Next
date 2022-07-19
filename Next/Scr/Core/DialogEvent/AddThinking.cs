using System;
using SkySwordKill.Next.DialogSystem;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("AddThinking")]
    public class AddThinking : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int id = command.GetInt(0);
            Tools.instance.getPlayer().wuDaoMag.AddLingGuangByJsonID(id);
            callback?.Invoke();
        }
    }
}