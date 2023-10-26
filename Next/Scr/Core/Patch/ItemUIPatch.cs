using Cysharp.Threading.Tasks;
using HarmonyLib;
using JSONClass;
using UnityEngine;

namespace SkySwordKill.Next.Patch;

/// <summary>
/// 物品图标Patch
/// </summary>
[HarmonyPatch(typeof(GUIPackage.item), "InitImage")]
public class ItemUIPatch
{
    public static int GetItemIconByKey(_ItemJsonData itemJsonData)
    {
        return itemJsonData.ItemIcon > 0 ? itemJsonData.ItemIcon : itemJsonData.id;
    }

    [HarmonyPostfix]
    public static void Postfix(GUIPackage.item __instance)
    {
        int id = __instance.itemID;
        if (id == -1) return;
        if (!_ItemJsonData.DataDict.ContainsKey(id))
        {
            return;
        }
        _ItemJsonData itemJsonData = _ItemJsonData.DataDict[id];
        var path = $"Assets/Item Icon/{GetItemIconByKey(itemJsonData)}.png";

        var texture = Main.Res.LoadAsset<Texture2D>(path);
        if (texture == null)
        {
            return;
        }
        
        __instance.itemIcon = texture;
        __instance.itemIconSprite = Main.Res.GetSpriteCache(texture);
    }
}