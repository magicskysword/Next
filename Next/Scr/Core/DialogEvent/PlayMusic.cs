using System;
using SkySwordKill.Next.DialogSystem;
using YSGame;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("PlayMusic")]
    public class PlayMusic : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var name = command.GetStr(0);

            MusicMag.instance.playMusic(name);
            callback?.Invoke();
        }
    }
}