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
            return player.Sex == 1 ? man : woman;
        }

        #endregion

        #region 私有方法



        #endregion


    }
}