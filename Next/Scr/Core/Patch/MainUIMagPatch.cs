using HarmonyLib;
using SkySwordKill.Next.ModGUI;
using SkySwordKill.Next.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SkySwordKill.Next.Patch;

/// <summary>
/// 主界面Patch
/// </summary>
[HarmonyPatch(typeof(MainUIMag))]
public class MainUIMagPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("OpenMain")]
    public static void AfterOpenUI(MainUIMag __instance)
    {
        var nextBtnGo = __instance.新主界面.transform.Find("Panel/btn/神仙斗法").gameObject.CopyGameObject(name : "Next");
        nextBtnGo.transform.MoveLocal(new Vector3(0, 90, 0));
        var nextBtn = nextBtnGo.GetComponent<FpBtn>();
        nextBtn.mouseUpEvent = new UnityEvent();
        nextBtn.mouseUpEvent.AddListener(() =>
        {
            var window = new ModMainWindow();
            window.Show();
        });
        
        var spr = Main.Res.LoadAsset<Texture2D>("Assets/Next/MCS_DLJM_btn_next.png").ToSprite();
        nextBtnGo.GetComponent<Image>().sprite = spr;
        nextBtn.nomalSprite = spr;
        
        spr = Main.Res.LoadAsset<Texture2D>("Assets/Next/MCS_DLJM_btn_next_bk.png").ToSprite();
        nextBtn.mouseDownSprite = spr;
        
        spr = Main.Res.LoadAsset<Texture2D>("Assets/Next/MCS_DLJM_btn_next_hlg.png").ToSprite();
        nextBtn.mouseEnterSprite = spr;

        nextBtnGo.AddComponent<MainPanelButtonAnimation>();

        Main.FGUI.ResetCamera();
    }
}