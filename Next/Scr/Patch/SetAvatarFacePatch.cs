using HarmonyLib;
using SkySwordKill.Next.StaticFace;
using YSGame;

namespace SkySwordKill.Next.Patch
{
    /// <summary>
    /// 游戏捏脸数据Patch
    /// </summary>
    [HarmonyPatch(typeof(SetAvatarFaceRandomInfo),"findStatic")]
    public class SetAvatarFacePatch
    {
        public static bool Prefix(SetAvatarFaceRandomInfo __instance,ref int __result, int avatarID,
            SetAvatarFaceRandomInfo.InfoName type)
        {

            if (StaticFaceUtils.HasFace(avatarID))
            {
                __result = StaticFaceUtils.GetFaceInfo(avatarID, type);
                return false;
            }
            
            return true;
        }
    }
}