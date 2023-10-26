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

        var audioClip = Main.Res.LoadAsset<AudioClip>($"Assets/Sound/Effect/{name}.mp3");
        if (audioClip != null)
        {
            MusicMag.instance.PlayEffectMusic(audioClip, pitch);
        }
        else
        {
            Debug.LogError($"音效 {name} 不存在。");
        }
        
        callback?.Invoke();
    }
}