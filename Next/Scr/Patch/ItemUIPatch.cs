using HarmonyLib;
using JSONClass;
using UnityEngine;

namespace SkySwordKill.Next.Patch
{
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
            if (Main.Instance.resourcesManager.TryGetAsset(path, asset =>
                {
                    if (asset is Texture2D texture)
                    {
                        __instance.itemIcon = texture;
                        __instance.itemIconSprite = Main.Instance.resourcesManager.GetSpriteCache(texture);
                    }
                }))
            {
                Main.LogInfo($"物品 [{__instance.itemID}] 图标加载成功");
            }
        }
    }
}