using System;
using KBEngine;

namespace SkySwordKill.Next
{
    public class DialogEnvironment
    {
        #region 字段

        public Avatar player;
        
        public int roleID;
        public int roleBindID;
        public string roleName;
        public UINPCData bindNpc;

        public int qiyuID;
        public int qiyuOption;

        #endregion

        #region 属性



        #endregion

        #region 回调方法



        #endregion

        #region 公共方法

        public DialogEnvironment()
        {
            player = Tools.instance.getPlayer();
        }

        public static int GetInt(string key)
        {
            return DialogAnalysis.GetInt(key);
        }

        public static string GetStr(string key)
        {
            return DialogAnalysis.GetStr(key);
        }

        public static bool Before(int year, int month = 12, int day = 31)
        {
            DateTime nowTime = Tools.instance.getPlayer().worldTimeMag.getNowTime();
            var tagTime = new DateTime(year, month, day);
            return tagTime > nowTime;
        }

        public static bool After(int year, int month = 1, int day = 1)
        {
            DateTime nowTime = Tools.instance.getPlayer().worldTimeMag.getNowTime();
            var tagTime = new DateTime(year, month, day);
            return tagTime < nowTime;
        }
        
        public static string GetCall(string man,string woman)
        {
            return Tools.instance.getPlayer().Sex == 1 ? man : woman;
        }
        
        public static string GetCall(Avatar avatar,string man,string woman)
        {
            return avatar.Sex == 1 ? man : woman;
        }

        public static ulong GetMoney() => Tools.instance.getPlayer().money;
        public static int GetHp() => Tools.instance.getPlayer().HP;
        public static int GetBaseHpMax() => Tools.instance.getPlayer()._HP_Max;
        public static int GetHpMax() => Tools.instance.getPlayer().HP_Max;
        public static int GetMentality() => Tools.instance.getPlayer().xinjin;
        public static int GetDrugsPoison() => Tools.instance.getPlayer().Dandu;
        public static int GetComprehensionPoint() => Tools.instance.getPlayer().WuDaoDian;
        public static int GetSex() => Tools.instance.getPlayer().Sex;
        public static int GetInspiration() => Tools.instance.getPlayer().LingGan;
        public static int GetInspirationMax() => Tools.instance.getPlayer().GetLingGanMax();
        public static uint GetAge() => Tools.instance.getPlayer().age;
        public static uint GetLife() => Tools.instance.getPlayer().shouYuan;
        public static int GetTalent() => Tools.instance.getPlayer().ZiZhi;
        public static int GetBaseSpirit() => Tools.instance.getPlayer()._shengShi;
        public static int GetSpirit() => Tools.instance.getPlayer().shengShi;
        public static uint GetAbility() => Tools.instance.getPlayer().wuXin;
        public static int GetBaseMoveSpeed() => Tools.instance.getPlayer()._dunSu;
        public static int GetMoveSpeed() => Tools.instance.getPlayer().dunSu;
        

        public static int GetComprehensionExp(int typeID)
        {
            return Tools.instance.getPlayer().wuDaoMag.getWuDaoEx(typeID).I;
        }

        public static int GetItemNum(int itemID)
        {
            return Tools.instance.getPlayer().getItemNum(itemID);
        }

        #endregion

        #region 私有方法



        #endregion


    }
}