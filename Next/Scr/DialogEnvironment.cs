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

        public int GetInt(string key)
        {
            return DialogAnalysis.GetInt(key);
        }

        public string GetStr(string key)
        {
            return DialogAnalysis.GetStr(key);
        }

        public bool Before(int year, int month = 12, int day = 31)
        {
            DateTime nowTime = Tools.instance.getPlayer().worldTimeMag.getNowTime();
            var tagTime = new DateTime(year, month, day);
            return tagTime > nowTime;
        }

        public bool After(int year, int month = 1, int day = 1)
        {
            DateTime nowTime = Tools.instance.getPlayer().worldTimeMag.getNowTime();
            var tagTime = new DateTime(year, month, day);
            return tagTime < nowTime;
        }
        
        public string GetCall(string man,string woman)
        {
            return Tools.instance.getPlayer().Sex == 1 ? man : woman;
        }
        
        public string GetCall(Avatar avatar,string man,string woman)
        {
            return avatar.Sex == 1 ? man : woman;
        }

        public ulong GetMoney() => Tools.instance.getPlayer().money;
        public int GetHp() => Tools.instance.getPlayer().HP;
        public int GetBaseHpMax() => Tools.instance.getPlayer()._HP_Max;
        public int GetHpMax() => Tools.instance.getPlayer().HP_Max;
        public int GetMentality() => Tools.instance.getPlayer().xinjin;
        public int GetDrugsPoison() => Tools.instance.getPlayer().Dandu;
        public int GetComprehensionPoint() => Tools.instance.getPlayer().WuDaoDian;
        public int GetSex() => Tools.instance.getPlayer().Sex;
        public int GetInspiration() => Tools.instance.getPlayer().LingGan;
        public int GetInspirationMax() => Tools.instance.getPlayer().GetLingGanMax();
        public uint GetAge() => Tools.instance.getPlayer().age;
        public uint GetLife() => Tools.instance.getPlayer().shouYuan;
        public int GetTalent() => Tools.instance.getPlayer().ZiZhi;
        public int GetBaseSpirit() => Tools.instance.getPlayer()._shengShi;
        public int GetSpirit() => Tools.instance.getPlayer().shengShi;
        public uint GetAbility() => Tools.instance.getPlayer().wuXin;
        public int GetBaseMoveSpeed() => Tools.instance.getPlayer()._dunSu;
        public int GetMoveSpeed() => Tools.instance.getPlayer().dunSu;
        

        public int GetComprehensionExp(int typeID)
        {
            return Tools.instance.getPlayer().wuDaoMag.getWuDaoEx(typeID).I;
        }

        public int GetItemNum(int itemID)
        {
            return Tools.instance.getPlayer().getItemNum(itemID);
        }

        #endregion

        #region 私有方法



        #endregion


    }
}