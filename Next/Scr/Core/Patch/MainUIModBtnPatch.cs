using HarmonyLib;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.ModGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SkySwordKill.Next.Patch;

[HarmonyPatch(typeof(MainUIMag), "Start")]
public class MainUIModBtnPatch
{
    [HarmonyPostfix]
    public static void Postfix(MainUIMag __instance)
    {
        var cloudSaveBtn = __instance.transform.Find("MainPanel/CloudSaveBtn");
        var nextModBtnTransform = Object.Instantiate(cloudSaveBtn, cloudSaveBtn.parent, true);
        nextModBtnTransform.name = "NextModBtn";
        nextModBtnTransform.localPosition = new Vector3(840f, -300f, 0);
        var nextModBtn = nextModBtnTransform.GetComponent<FpBtn>();
        nextModBtn.mouseDownEvent = new UnityEvent();
        nextModBtn.MouseUp = null;
        nextModBtn.mouseUpEvent = new UnityEvent();
        nextModBtn.mouseUpEvent.AddListener(() =>
        {
            var window = new ModMainWindow();
            window.Show();
        });
        var nextModBtnText = nextModBtnTransform.Find("Text").GetComponent<Text>();
        nextModBtnText.text = "Next Mod";
    }
    
}