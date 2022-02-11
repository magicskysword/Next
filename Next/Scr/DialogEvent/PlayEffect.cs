using System;
using UnityEngine;
using YSGame;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("PlayMusic")]
    public class PlayEffect : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var name = command.GetStr(0);
            var pitch = command.GetFloat(1, 1f);
            
            if (Main.Instance.resourcesManager.TryGetAsset($"Assets/Sound/Effect/{name}.mp3", out var asset))
            {
                if (asset is AudioClip audioClip)
                    MusicMag.instance.PlayEffectMusic(audioClip, pitch);
            }
            callback?.Invoke();
        }
    }
}