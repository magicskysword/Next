using HarmonyLib;
using JSONClass;
using KBEngine;
using SkySwordKill.Next.Utils;
using UnityEngine;
using YSGame.Fight;

namespace SkySwordKill.Next.Patch;

/// <summary>
/// 战斗界面武器图标补充Patch
/// </summary>
[HarmonyPatch(typeof(UIFightWeaponItem),"SetWeapon")]
public class UIFightWeaponItemPatch
{
    [HarmonyPostfix]
    public static void Postfix(UIFightWeaponItem __instance,GUIPackage.Skill skill, ITEM_INFO weapon)
    {
        int id = weapon.itemId;
        if (id == -1) return;
        if (_ItemJsonData.DataDict.TryGetValue(id, out var itemJsonData))
        {
            var path = $"Assets/Item Icon/{ItemUIPatch.GetItemIconByKey(itemJsonData)}.png";

            if (!Main.Res.HaveAsset(path))
                path = $"Assets/Item Icon/1.png";
                
            
            var texture = Main.Res.LoadAsset<Texture2D>(path);
            if (texture == null)
            {
                return;
            }
            
            __instance.IconImage.sprite = texture.ToSprite();
            Main.LogInfo($"物品 [{id}] 图标加载成功");
        }
    }
}