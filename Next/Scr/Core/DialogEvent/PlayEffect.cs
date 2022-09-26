using System;
using SkySwordKill.Next.DialogSystem;
using UnityEngine;
using YSGame;

namespace SkySwordKill.Next.DialogEvent;

[DialogEvent("PlayEffect")]
public class PlayEffect : IDialogEvent
{
    public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
    {
        var name = command.GetStr(0);
        var pitch = command.GetFloat(1, 1f);

        Main.Res.TryGetAsset($"Assets/Sound/Effect/{name}.mp3", asset =>
        {
            if (asset is AudioClip audioClip)
                MusicMag.instance.PlayEffectMusic(audioClip, pitch);
        });
        callback?.Invoke();
    }
}