using System;
using System.Collections.Generic;
using System.Linq;
using Fungus;
using KBEngine;
using SkySwordKill.Next;

namespace SkySwordKill.Next.DialogSystem
{
    /// <summary>
    /// 脚本环境类
    /// 该类中的变量与函数不可轻易改名
    /// 其对应 运行时脚本 中的变量与方法
    /// </summary>
    public class DialogEnvironment
    {
        #region 字段
        
        public string curDialogID;
        public int curDialogIndex = 0;

        public Avatar player;
        
        public int roleID;
        public int roleBindID;
        public string roleName = string.Empty;
        public UINPCData bindNpc;

        public int qiyuID;
        public int qiyuOption;

        public int itemID;

        public string[] fightTags;
        public string input = string.Empty;

        public int optionID;
        
        public Flowchart flowchart;

        public Dictionary<string, int> tmpArgs = new Dictionary<string, int>();

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
        
        public DialogEnvironment Clone()
        {
            var newEnv = new DialogEnvironment();

            // 反射复制字段
            // TODO: 性能较低，待改进
            foreach (var fieldInfo in typeof(DialogEnvironment).GetFields())
            {
                fieldInfo.SetValue(newEnv,fieldInfo.GetValue(this));
            }

            return newEnv;
        }

        public int GetArg(string argKey)
        {
            if (tmpArgs.TryGetValue(argKey, out var value))
            {
                return value;
            }

            return 0;
        }

        public int GetLuaInt(string luaFile, string luaFunc)
        {
            var rets = Main.Lua.RunFunc(luaFile, luaFunc,
                new object[] { this });
            if (rets != null && rets.Length > 0)
            {
                return Convert.ToInt32(rets[0]);
            }

            return 0;
        }
        
        public string GetLuaStr(string luaFile, string luaFunc)
        {
            var rets = Main.Lua.RunFunc(luaFile, luaFunc,
                new object[] { this });
            if (rets != null && rets.Length > 0)
            {
                return Convert.ToString(rets[0]);
            }

            return string.Empty;
        }
        
        public int Random(int minInclude, int maxExclude)
        {
            return UnityEngine.Random.Range(minInclude, maxExclude);
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
            DateTime nowTime = player.worldTimeMag.getNowTime();
            var tagTime = new DateTime(year, month, day);
            return tagTime > nowTime;
        }

        public bool After(int year, int month = 1, int day = 1)
        {
            DateTime nowTime = player.worldTimeMag.getNowTime();
            var tagTime = new DateTime(year, month, day);
            return tagTime < nowTime;
        }

        public DateTime GetDateTime(int year, int month = 1, int day = 1)
        {
            return new DateTime(year, month, day);
        }
        
        public DateTime GetNowTime()
        {
            return player.worldTimeMag.getNowTime();
        }

        public bool HasSkill(int skillID)
        {
            return player.hasSkillList.Find(skill => skill.itemId == skillID) != null;
        }
        
        public bool HasStaticSkill(int skillID)
        {
            return player.hasStaticSkillList.Find(skill => skill.itemId == skillID) != null;
        }
        
        public bool HasTrainSkill(int skillID)
        {
            return PlayerEx.HasShuangXiuSkill(skillID);
        }
        
        public string GetCall(string man,string woman)
        {
            return player.Sex == 1 ? man : woman;
        }

        public string GetCurScene() => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        public string GetCurMapRoad() => player.NowMapIndex.ToString();
        public string GetSceneName(string sceneID) => DialogAnalysis.GetSceneName(sceneID);
        public string GetRoadName(string roadId) => DialogAnalysis.GetMapRoadName(roadId);
        public ulong GetMoney() => player.money;
        public int GetHp() => player.HP;
        public int GetBaseHpMax() => player._HP_Max;
        public int GetHpMax() => player.HP_Max;
        public int GetMentality() => player.xinjin;
        public int GetDrugsPoison() => player.Dandu;
        public int GetComprehensionPoint() => player.WuDaoDian;
        public int GetSex() => player.Sex;
        public int GetInspiration() => player.LingGan;
        public int GetInspirationMax() => player.GetLingGanMax();
        public uint GetAge() => player.age;
        public uint GetLife() => player.shouYuan;
        public int GetTalent() => player.ZiZhi;
        public int GetBaseSpirit() => player._shengShi;
        public int GetSpirit() => player.shengShi;
        public uint GetAbility() => player.wuXin;
        public int GetBaseMoveSpeed() => player._dunSu;
        public int GetMoveSpeed() => player.dunSu;
        public int GetLevel() => player.level;
        public int GetLevelType() => player.getLevelType();
        

        public int GetComprehensionExp(int typeID)
        {
            return player.wuDaoMag.getWuDaoEx(typeID).I;
        }

        public int GetCongenitalBuffCount(int buffID)
        {
            var seid16 = 16.ToString();
            if (player.TianFuID.HasField(seid16))
            {
                var list = player.TianFuID[seid16].ToList();
                return list.Count(buff => buff == buffID);
            }
            return 0;
        }

        public int GetItemNum(int _itemID)
        {
            return player.getItemNum(_itemID);
        }
        
        public int GetNpcFav(int npcId) => NPCEx.GetFavor(NPCEx.NPCIDToNew(npcId));
        public int GetNpcSex(int npcId) => DialogAnalysis.GetNpcSex(npcId);
        public int GetNpcAge(int npcId) => DialogAnalysis.GetNpcAge(npcId);
        public int GetNpcLife(int npcId) => DialogAnalysis.GetNpcLife(npcId);
        public int GetNpcLevel(int npcId) => DialogAnalysis.GetNpcLevel(npcId);
        public int GetNpcLevelType(int npcId) => DialogAnalysis.GetNpcLevelType(npcId);
        public int GetNpcSprite(int npcId) => DialogAnalysis.GetNpcSprite(npcId);
        public bool IsNpcDeath(int npcId) => NPCEx.IsDeath(NPCEx.NPCIDToNew(npcId));
        public bool IsCouple(int npcId) => DialogAnalysis.IsPlayerCouple(npcId);
        public bool IsTeacher(int npcId) => DialogAnalysis.IsPlayerTeacher(npcId);
        public bool IsBrother(int npcId) => DialogAnalysis.IsPlayerBrother(npcId);
        public bool IsStudent(int npcId) => DialogAnalysis.IsPlayerStudent(npcId);



        #endregion

        #region 私有方法



        #endregion
    }
}