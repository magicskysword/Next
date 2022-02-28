using System;
using System.Collections.Generic;
using YSGame;

namespace SkySwordKill.Next.StaticFace
{
    public static class StaticFaceUtils
    {
        public static Dictionary<int, CustomStaticFaceInfo> FaceInfos { get; set; } = new Dictionary<int, CustomStaticFaceInfo>();

        public static void Clear()
        {
            FaceInfos.Clear();
        }

        public static bool HasFace(int avatarId) => FaceInfos.ContainsKey(avatarId);

        public static CustomStaticFaceInfo GetFace(int avatarId) => FaceInfos[avatarId];
        
        public static int GetFaceInfo(int avatarId, SetAvatarFaceRandomInfo.InfoName type)
        {
            var typeName = type.ToString();

            if (FaceInfos.TryGetValue(avatarId, out var info))
            {
                return info.GetRandomInfo(typeName);
            }

            return -100;
        }

        public static void RegisterFace(CustomStaticFaceInfo faceInfo)
        {
            FaceInfos[faceInfo.ID] = faceInfo;
        }

        public static CustomStaticFaceInfo GetFaceInfoByJson(JSONObject jsonObject)
        {
            var faceInfo = new CustomStaticFaceInfo();
            foreach (var field in jsonObject.keys)
            {
                if (Enum.TryParse<SetAvatarFaceRandomInfo.InfoName>(field, true, out _))
                {
                    faceInfo.RandomInfos[field] = jsonObject[field].I;
                }
            }

            return faceInfo;
        }
        
        public static CustomStaticFaceInfo GetFaceInfoByStaticFaceInfo(StaticFaceInfo staticFaceInfo)
        {
            var faceInfo = new CustomStaticFaceInfo();
            foreach (var faceRandomInfo in staticFaceInfo.faceinfoList)
            {
                faceInfo.RandomInfos[faceRandomInfo.SkinTypeName.ToString()] = faceRandomInfo.SkinTypeScope;
            }

            return faceInfo;
        }
    }
}