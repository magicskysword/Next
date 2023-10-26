﻿using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using YSGame;

namespace SkySwordKill.Next.Patch;

/// <summary>
/// 游戏音乐加载Patch
/// </summary>
[HarmonyPatch(typeof(MusicMag))]
public class MusicMagPatch
{
    public static Dictionary<string,MusicInfo> Background = null;

    [HarmonyPatch("getMusicIndex")]
    [HarmonyPrefix]
    public static void Prefix(MusicMag __instance,string name)
    {
        if (Background == null)
            Background = __instance.BackGroudMusic.ToDictionary(info => info.name);
            
        var musicInfo = __instance.BackGroudMusic.Find(info => info.name == name);
        var audioClip = Main.Res.LoadAsset<AudioClip>($"Assets/Sound/Music/{name}.mp3");
        if (audioClip == null)
        {
            if (Background.TryGetValue(name, out var rawMusic))
            {
                musicInfo.audioClip = rawMusic.audioClip;
            }
        }
        else
        {
            if (musicInfo != null)
            {
                musicInfo.audioClip = audioClip;
            }
            else
            {
                musicInfo = new MusicInfo()
                {
                    name = name,
                    audioClip = audioClip
                };
                __instance.BackGroudMusic.Add(musicInfo);
            }
        }
    }
}