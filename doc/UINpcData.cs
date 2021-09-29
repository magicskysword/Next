// Decompiled with JetBrains decompiler
// Type: UINPCData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E2B6E792-A04A-4BD1-BCE3-D03DCBE9B960
// Assembly location: D:\soft\steam\steamapps\common\觅长生\觅长生test_Data\Managed\Assembly-CSharp.dll

using GUIPackage;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UINPCData : IComparable
{
    private static bool isDebugMode = false;
    public static Dictionary<int, UnityAction> ThreeSceneNPCTalkCache = new Dictionary<int, UnityAction>();
    public static Dictionary<int, UnityAction> ThreeSceneZhongYaoNPCTalkCache = new Dictionary<int, UnityAction>();
    public bool IsException;
    public int ID;
    public string UUID = "";
    public int Tag;
    public bool IsGuDingNPC;
    public bool IsZhongYaoNPC;
    public int ZhongYaoNPCID;
    public bool IsThreeSceneNPC;
    public bool IsBind;
    public string Name;
    public int Sex;
    public string Title;
    public int Age;
    public int HP;
    public int QingFen;
    public int Exp;
    public int Level;
    public int BigLevel;
    public string LevelStr;
    public int ZhuangTai;
    public string ZhuangTaiStr;
    public int ShouYuan;
    public int ZiZhi;
    public int WuXing;
    public int DunSu;
    public int ShenShi;
    public int Favor;
    public int FavorLevel;
    public float FavorPer;
    public bool IsNingZhouNPC;
    public bool IsKnowPlayer;
    public int XingGe;
    public int LiuPai;
    public int MenPai;
    public int ActionID;
    public bool IsTanChaUnlock;
    public bool IsNeedHelp;
    public bool IsTask;
    public bool IsSeaNPC;
    public bool IsTag;
    public int SeaEventID;
    public int NPCType;
    public bool ZhengXie;
    public int Face;
    public List<UINPCEventData> Events = new List<UINPCEventData>();
    public JSONObject json;
    public JSONObject Weapon1;
    public JSONObject Weapon2;
    public JSONObject Clothing;
    public JSONObject Ring;
    public bool IsDoubleWeapon;
    public List<int> StaticSkills = new List<int>();
    public int YuanYingStaticSkill;
    public List<int> Skills = new List<int>();
    public List<UINPCWuDaoData> WuDao = new List<UINPCWuDaoData>();
    public List<int> WuDaoSkills = new List<int>();
    public bool IsFight;
    public JSONObject BackpackJson;
    public List<item> Inventory = new List<item>();
    private static Dictionary<int, string> _LevelDict = new Dictionary<int, string>();
    private static Dictionary<int, string> _ZhuangTaiDict = new Dictionary<int, string>();
    private static Dictionary<int, Dictionary<int, int>> _SkillDict = new Dictionary<int, Dictionary<int, int>>();
    public static Dictionary<int, UIWuDaoSkillData> _WuDaoSkillDict = new Dictionary<int, UIWuDaoSkillData>();
    private static Dictionary<int, List<int>> _WuDaoSkillTypeDict = new Dictionary<int, List<int>>();
    private static Dictionary<int, string> _EventDescDict = new Dictionary<int, string>();
    private static Dictionary<int, string> _EventGuDingDescDict = new Dictionary<int, string>();
    private static Dictionary<int, int> _EventTypeDict = new Dictionary<int, int>();
    private static Dictionary<int, string> _EventQiYuDescDict = new Dictionary<int, string>();
    private static Dictionary<int, bool> _ActionTaskDict = new Dictionary<int, bool>();
    private static bool _Inited;
    private static string[] fabaoleixing = new string[3]
    {
    "武器",
    "防具",
    "饰品"
    };
    private static string[] fabaopinjie = new string[5]
    {
    "符器",
    "法器",
    "法宝",
    "纯阳法宝",
    "通天灵宝"
    };
    private static string[] dajingjie = new string[5]
    {
    "炼气",
    "筑基",
    "金丹",
    "元婴",
    "化神"
    };
    private static string[] xiaojingjie = new string[3]
    {
    "初期",
    "中期",
    "后期"
    };
    private static Dictionary<int, string> _TypeWuDaoName = new Dictionary<int, string>()
  {
    {
      1,
      "金"
    },
    {
      2,
      "木"
    },
    {
      3,
      "水"
    },
    {
      4,
      "火"
    },
    {
      5,
      "土"
    },
    {
      6,
      "神"
    },
    {
      7,
      "体"
    },
    {
      8,
      "剑"
    },
    {
      9,
      "气"
    },
    {
      10,
      "阵"
    },
    {
      21,
      "丹"
    },
    {
      22,
      "器"
    }
  };

    private static void Init()
    {
        if (UINPCData._Inited)
            return;
        foreach (JSONObject jsonObject in jsonData.instance.LevelUpDataJsonData.list)
            UINPCData._LevelDict.Add(jsonObject["id"].I, jsonObject["Name"].Str);
        foreach (JSONObject jsonObject in jsonData.instance.NpcStatusDate.list)
            UINPCData._ZhuangTaiDict.Add(jsonObject["id"].I, jsonObject["ZhuangTaiInfo"].Str);
        foreach (JSONObject jsonObject in jsonData.instance._skillJsonData.list)
        {
            int i1 = jsonObject["Skill_ID"].I;
            int i2 = jsonObject["Skill_Lv"].I;
            int i3 = jsonObject["id"].I;
            if (UINPCData._SkillDict.ContainsKey(i1))
                UINPCData._SkillDict[i1].TryAdd<int, int>(i2, i3);
            else
                UINPCData._SkillDict.TryAdd<int, Dictionary<int, int>>(i1, new Dictionary<int, int>()
        {
          {
            i2,
            i3
          }
        });
        }
        foreach (JSONObject jsonObject1 in jsonData.instance.WuDaoJson.list)
        {
            UIWuDaoSkillData uiWuDaoSkillData = new UIWuDaoSkillData();
            uiWuDaoSkillData.ID = jsonObject1["id"].I;
            uiWuDaoSkillData.Name = jsonObject1["name"].Str;
            uiWuDaoSkillData.WuDaoLv = jsonObject1["Lv"].I;
            UINPCData._WuDaoSkillTypeDict.Add(uiWuDaoSkillData.ID, new List<int>());
            foreach (JSONObject jsonObject2 in jsonObject1["Type"].list)
            {
                uiWuDaoSkillData.WuDaoType.Add(jsonObject2.I);
                UINPCData._WuDaoSkillTypeDict[uiWuDaoSkillData.ID].Add(jsonObject2.I);
            }
            uiWuDaoSkillData.Desc = jsonObject1["xiaoguo"].Str;
            UINPCData._WuDaoSkillDict.Add(uiWuDaoSkillData.ID, uiWuDaoSkillData);
        }
        foreach (JSONObject jsonObject in jsonData.instance.NpcShiJianData.list)
        {
            UINPCData._EventDescDict.Add(jsonObject["id"].I, jsonObject["ShiJianInfo"].Str);
            UINPCData._EventTypeDict.Add(jsonObject["id"].I, jsonObject["ShiJianType"].I);
        }
        foreach (JSONObject jsonObject in jsonData.instance.NpcQiYuDate.list)
            UINPCData._EventQiYuDescDict.Add(jsonObject["id"].I, jsonObject["QiYuInfo"].Str);
        foreach (JSONObject jsonObject in jsonData.instance.NpcImprotantEventData.list)
            UINPCData._EventGuDingDescDict.Add(jsonObject["id"].I, jsonObject["ShiJianInfo"].Str);
        foreach (JSONObject jsonObject in jsonData.instance.NPCActionDate.list)
            UINPCData._ActionTaskDict.Add(jsonObject["id"].I, jsonObject["IsTask"].I == 1);
        UINPCData._Inited = true;
    }

    public UINPCData(int id, bool isThreeSceneNPC = false)
    {
        this.ID = id;
        this.IsThreeSceneNPC = isThreeSceneNPC;
    }

    public void SetID(int id)
    {
        this.ID = id;
        this.RefreshData();
    }

    public void RefreshData()
    {
        UINPCData.Init();
        try
        {
            this.json = this.ID.NPCJson();
            if (jsonData.instance.AvatarRandomJsonData.HasField(this.ID.ToString()))
            {
                this.Name = jsonData.instance.AvatarRandomJsonData[this.ID.ToString()]["Name"].Str;
                this.Favor = jsonData.instance.AvatarRandomJsonData[this.ID.ToString()]["HaoGanDu"].I;
            }
            else
            {
                this.Name = "获取失败";
                Debug.LogError((object)string.Format("获取NPC {0} 的名字和好感度失败，jsonData.instance.AvatarRandomJsonData中没有此ID", (object)this.ID));
            }
            if (UINPCJiaoHu.isDebugMode)
                this.Name += this.ID.ToString();
            this.Sex = this.json["SexType"].I;
            this.Title = this.json["Title"].Str;
            this.Age = this.json["age"].I / 12;
            this.HP = this.json["HP"].I;
            if (jsonData.instance.AvatarJsonData.HasField(this.ID.ToString()))
                this.Face = jsonData.instance.AvatarJsonData[this.ID.ToString()]["face"].I;
            this.IsTag = this.json.TryGetField("IsTag").b;
            if (UINPCData.isDebugMode)
                this.Favor = UnityEngine.Random.Range(-100, 300);
            this.FavorLevel = UINPCHeadFavor.GetFavorLevel(this.Favor);
            this.Level = this.json["Level"].I;
            this.LevelStr = UINPCData._LevelDict[this.Level];
            this.BigLevel = (this.Level - 1) / 3 + 1;
            int num1 = NPCEx.CalcZengLiX(this);
            this.FavorPer = (float)PlayerEx.Player.ZengLi.TryGetField("DuoYuQingFen").TryGetField(this.ID.ToString()).I / (float)num1;
            if (!this.json.HasField("isImportant"))
                return;
            if (this.json["isImportant"].b)
                this.IsGuDingNPC = true;
            this.IsZhongYaoNPC = NPCEx.IsZhongYaoNPC(this.ID, out this.ZhongYaoNPCID);
            int num2 = this.IsZhongYaoNPC ? 1 : 0;
            this.NPCType = this.json["Type"].I;
            this.Tag = this.json["NPCTag"].I;
            this.ZhengXie = jsonData.instance.NPCTagDate[this.Tag.ToString()]["zhengxie"].I == 1;
            this.Exp = this.json["exp"].I;
            this.QingFen = this.json["QingFen"].I;
            this.ZhuangTai = this.json["Status"]["StatusId"].I;
            this.ZhuangTaiStr = UINPCData._ZhuangTaiDict[this.ZhuangTai];
            this.ShouYuan = this.json["shouYuan"].I;
            this.ZiZhi = this.json["ziZhi"].I;
            this.WuXing = this.json["wuXin"].I;
            this.DunSu = this.json["dunSu"].I;
            this.ShenShi = this.json["shengShi"].I;
            this.IsKnowPlayer = this.json["IsKnowPlayer"].b;
            this.IsNingZhouNPC = this.json["paimaifenzu"].list.Count != 2 && this.json["paimaifenzu"].list[0].I != 2;
            this.XingGe = this.json["XingGe"].I;
            this.LiuPai = this.json["LiuPai"].I;
            this.MenPai = this.json["MenPai"].I;
            this.ActionID = this.json["ActionId"].I;
            this.IsNeedHelp = this.json["IsNeedHelp"].b;
            this.IsTask = false;
            if (this.IsNeedHelp && !SceneManager.GetActiveScene().name.StartsWith("Sea") && this.FavorLevel >= 3 && UINPCData._ActionTaskDict.ContainsKey(this.ActionID) && UINPCData._ActionTaskDict[this.ActionID] && PlayerEx.Player.getLevelType() >= this.BigLevel - 1)
                this.IsTask = true;
            this.IsTanChaUnlock = this.json.HasField("isTanChaUnlock") && this.json["isTanChaUnlock"].b;
            JSONObject jsonObject1 = this.json["equipList"];
            if (jsonObject1.HasField("Weapon1"))
            {
                this.Weapon1 = jsonObject1["Weapon1"];
                if (jsonObject1.HasField("Weapon2"))
                {
                    this.Weapon2 = jsonObject1["Weapon2"];
                    this.IsDoubleWeapon = true;
                }
            }
            if (jsonObject1.HasField("Clothing"))
                this.Clothing = jsonObject1["Clothing"];
            if (jsonObject1.HasField("Ring"))
                this.Ring = jsonObject1["Ring"];
            this.StaticSkills.Clear();
            foreach (JSONObject jsonObject2 in this.json["staticSkills"].list)
                this.StaticSkills.Add(jsonObject2.I);
            this.YuanYingStaticSkill = this.json["yuanying"].I;
            this.Skills.Clear();
            foreach (JSONObject jsonObject3 in this.json["skills"].list)
                this.Skills.Add(UINPCData._SkillDict[jsonObject3.I][this.BigLevel]);
            this.WuDaoSkills.Clear();
            if (!this.json["wuDaoSkillList"].IsNull)
            {
                foreach (JSONObject jsonObject4 in this.json["wuDaoSkillList"].list)
                    this.WuDaoSkills.Add(jsonObject4.I);
            }
            this.WuDao = new List<UINPCWuDaoData>();
            foreach (JSONObject jsonObject5 in this.json["wuDaoJson"].list)
            {
                UINPCWuDaoData uinpcWuDaoData = new UINPCWuDaoData();
                uinpcWuDaoData.ID = jsonObject5["id"].I;
                uinpcWuDaoData.Level = jsonObject5["level"].I;
                uinpcWuDaoData.Exp = jsonObject5["exp"].I;
                foreach (KeyValuePair<int, UIWuDaoSkillData> keyValuePair in UINPCData._WuDaoSkillDict)
                {
                    if (this.WuDaoSkills.Contains(keyValuePair.Key) && keyValuePair.Value.WuDaoType.Contains(uinpcWuDaoData.ID))
                    {
                        uinpcWuDaoData.SkillIDList.Add(keyValuePair.Key);
                        if (keyValuePair.Value.WuDaoLv > uinpcWuDaoData.Level)
                            Debug.LogError((object)string.Format("解析NPC数据时出现问题，NPC的悟道技能超过了悟道等级，NPCID:{0}，悟道技能ID:{1}", (object)this.ID, (object)keyValuePair.Key));
                    }
                }
                this.WuDao.Add(uinpcWuDaoData);
            }
            this.WuDao.Sort();
            this.Events.Clear();
            this.ParseEvent();
            if (!jsonData.instance.AvatarBackpackJsonData.HasField(this.ID.ToString()))
                return;
            this.BackpackJson = jsonData.instance.AvatarBackpackJsonData[this.ID.ToString()];
            this.Inventory.Clear();
            foreach (JSONObject jsonObject6 in this.BackpackJson["Backpack"].list)
            {
                int i = jsonObject6["Num"].I;
                if (i > 0)
                    this.Inventory.Add(new item(jsonObject6["ItemID"].I)
                    {
                        UUID = jsonObject6["UUID"].str,
                        Seid = !jsonObject6.HasField("Seid") ? new JSONObject(JSONObject.Type.OBJECT) : jsonObject6["Seid"],
                        itemNum = i
                    });
            }
        }
        catch (Exception ex)
        {
            this.IsException = true;
            Debug.LogError((object)string.Format("获取NPC数据时出错，目标的ID:{0}，错误信息:\n{1}", (object)this.ID, (object)ex));
        }
    }

    public void RefreshOldNpcData()
    {
        UINPCData.Init();
        try
        {
            this.json = this.ID.NPCJson();
            if (jsonData.instance.AvatarRandomJsonData.HasField(this.ID.ToString()))
            {
                this.Name = jsonData.instance.AvatarRandomJsonData[this.ID.ToString()]["Name"].Str;
                this.Favor = 0;
            }
            else
            {
                this.Name = "获取失败";
                Debug.LogError((object)string.Format("获取NPC {0} 的名字和好感度失败，jsonData.instance.AvatarRandomJsonData中没有此ID", (object)this.ID));
            }
            if (UINPCJiaoHu.isDebugMode)
                this.Name += this.ID.ToString();
            this.Sex = this.json["SexType"].I;
            this.Title = this.json["Title"].Str;
            this.Age = this.json["age"].I / 12;
            this.HP = this.json["HP"].I;
            this.FavorLevel = UINPCHeadFavor.GetFavorLevel(this.Favor);
            this.Level = this.json["Level"].I;
            this.LevelStr = UINPCData._LevelDict[this.Level];
            this.BigLevel = (this.Level - 1) / 3 + 1;
            this.FavorPer = 0.0f;
            this.Exp = 0;
            this.QingFen = 0;
            this.ZhuangTai = 1;
            this.ZhuangTaiStr = UINPCData._ZhuangTaiDict[this.ZhuangTai];
            this.ShouYuan = this.json["shouYuan"].I;
            this.ZiZhi = this.json["ziZhi"].I;
            this.WuXing = this.json["wuXin"].I;
            this.DunSu = this.json["dunSu"].I;
            this.ShenShi = this.json["shengShi"].I;
            JSONObject jsonObject1 = this.json["equipList"];
            if (this.json["equipWeapon"].I > 0)
            {
                this.Weapon1 = new JSONObject();
                this.Weapon1.SetField("ItemID", this.json["equipWeapon"].I);
            }
            if (this.json["equipClothing"].I > 0)
            {
                this.Clothing = new JSONObject();
                this.Clothing.SetField("ItemID", this.json["equipClothing"].I);
            }
            if (this.json["equipRing"].I > 0)
            {
                this.Ring = new JSONObject();
                this.Ring.SetField("ItemID", this.json["equipRing"].I);
            }
            this.StaticSkills.Clear();
            foreach (JSONObject jsonObject2 in this.json["staticSkills"].list)
                this.StaticSkills.Add(jsonObject2.I);
            this.Skills.Clear();
            foreach (JSONObject jsonObject3 in this.json["skills"].list)
                this.Skills.Add(UINPCData._SkillDict[jsonObject3.I][this.BigLevel]);
        }
        catch (Exception ex)
        {
            this.IsException = true;
            Debug.LogError((object)string.Format("获取NPC数据时出错，目标的ID:{0}，错误信息:\n{1}", (object)this.ID, (object)ex));
        }
    }

    public void ParseEvent()
    {
        JSONObject jsonObject1 = this.json["NoteBook"];
        if (jsonObject1.IsNull)
            return;
        foreach (string key1 in jsonObject1.keys)
        {
            int.Parse(key1);
            foreach (JSONObject jsonObject2 in jsonObject1[key1].list)
            {
                UINPCEventData uinpcEventData = new UINPCEventData();
                uinpcEventData.EventDesc = "";
                uinpcEventData.EventTime = DateTime.Parse(jsonObject2["time"].str);
                uinpcEventData.EventTimeStr = jsonObject2["time"].str;
                if (key1 == "33")
                    uinpcEventData.EventDesc += UINPCData._EventQiYuDescDict[jsonObject2["qiYuId"].I];
                else if (key1 == "101")
                {
                    uinpcEventData.EventDesc += UINPCData._EventGuDingDescDict[jsonObject2["gudingshijian"].I];
                }
                else
                {
                    int key2 = int.Parse(key1);
                    if (UINPCData._EventDescDict.ContainsKey(key2))
                        uinpcEventData.EventDesc += UINPCData._EventDescDict[key2];
                    else
                        Debug.LogError((object)string.Format("解析重要事件出错，配表中没有id为{0}重要事件", (object)key2));
                }
                foreach (string key3 in jsonObject2.keys)
                {
                    if (key3 == "num")
                        uinpcEventData.EventDesc = uinpcEventData.EventDesc.Replace("{num}", jsonObject2[key3].I.ToString());
                    else if (key3 == "danyao")
                    {
                        JSONObject jsonObject3 = jsonObject2[key3].I.ItemJson();
                        string str = jsonObject3["name"].Str;
                        if (str == "废丹")
                        {
                            uinpcEventData.EventDesc = "在炼制丹药时由于药引错误，药性未能中和，仅炼制成{Num}颗{danyao}。";
                            uinpcEventData.EventDesc = uinpcEventData.EventDesc.Replace("{danyao}", str);
                            uinpcEventData.EventDesc = uinpcEventData.EventDesc.Replace("{Num}", jsonObject2["num"].I.ToString());
                            break;
                        }
                        uinpcEventData.EventDesc = uinpcEventData.EventDesc.Replace("{danyao}", str);
                        uinpcEventData.EventDesc = uinpcEventData.EventDesc.Replace("{pinjie}", jsonObject3["quality"].I.ToCNNumber() + "品");
                    }
                    else if (key3 == "jingjie")
                        uinpcEventData.EventDesc = uinpcEventData.EventDesc.Replace("{jingjie}", UINPCData.dajingjie[(jsonObject2[key3].I - 1) / 3] + UINPCData.xiaojingjie[(jsonObject2[key3].I - 1) % 3]);
                    else if (key3 == "leixing")
                        uinpcEventData.EventDesc = uinpcEventData.EventDesc.Replace("{leixing}", UINPCData.fabaoleixing[jsonObject2[key3].I - 1]);
                    else if (key3 == "fabaopinjie")
                        uinpcEventData.EventDesc = uinpcEventData.EventDesc.Replace("{fabaopinjie}", UINPCData.fabaopinjie[jsonObject2[key3].I - 1]);
                    else if (key3 == "zhuangbei")
                        uinpcEventData.EventDesc = uinpcEventData.EventDesc.Replace("{zhuangbei}", jsonObject2[key3].Str);
                    else if (key3 == "item")
                    {
                        string str = jsonObject2[key3].I.ItemJson()["name"].Str;
                        if (jsonObject2.HasField("itemName") && jsonObject2["itemName"].Str != "")
                            str = jsonObject2["itemName"].Str;
                        uinpcEventData.EventDesc = uinpcEventData.EventDesc.Replace("{item}", str);
                    }
                    else if (key3 == "npcname")
                        uinpcEventData.EventDesc = uinpcEventData.EventDesc.Replace("{npcname}", jsonObject2[key3].Str);
                    else if (key3 == "fungusshijian")
                        uinpcEventData.EventDesc = uinpcEventData.EventDesc.Replace("{fungusshijian}", jsonObject2[key3].Str);
                    else if (key3 == "cnnum")
                        uinpcEventData.EventDesc = uinpcEventData.EventDesc.Replace("{cnnum}", jsonObject2[key3].I.ToCNNumber());
                }
                uinpcEventData.EventDesc = uinpcEventData.EventDesc.Replace("{FirstName}", PlayerEx.Player.firstName);
                uinpcEventData.EventDesc = uinpcEventData.EventDesc.Replace("{LastName}", PlayerEx.Player.lastName);
                this.Events.Add(uinpcEventData);
            }
        }
        try
        {
            this.Events.Sort();
        }
        catch (Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("排序NPC重要时间时出错");
            stringBuilder.AppendLine(ex.Message);
            stringBuilder.AppendLine(ex.StackTrace);
            foreach (UINPCEventData uinpcEventData in this.Events)
                stringBuilder.AppendLine(string.Format("{0} {1}", (object)uinpcEventData.EventTime, (object)uinpcEventData.EventDesc));
            Debug.LogError((object)stringBuilder.ToString());
        }
    }

    public static void CheckWuDaoError()
    {
        UINPCData.Init();
        StringBuilder stringBuilder = new StringBuilder();
        foreach (JSONObject jsonObject1 in jsonData.instance.NPCWuDaoJson.list)
        {
            int i1 = jsonObject1["id"].I;
            int i2 = jsonObject1["Type"].I;
            int i3 = jsonObject1["lv"].I;
            List<int> intList = new List<int>();
            foreach (JSONObject jsonObject2 in jsonObject1["wudaoID"].list)
                intList.Add(jsonObject2.I);
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            for (int key = 1; key <= 12; ++key)
            {
                int i4 = jsonObject1[string.Format("value{0}", (object)key)].I;
                if (key > 10)
                    dictionary.Add(key + 10, i4);
                else
                    dictionary.Add(key, i4);
            }
            bool flag = false;
            foreach (int key1 in intList)
            {
                UIWuDaoSkillData uiWuDaoSkillData = UINPCData._WuDaoSkillDict[key1];
                foreach (int key2 in uiWuDaoSkillData.WuDaoType)
                {
                    if (uiWuDaoSkillData.WuDaoLv > dictionary[key2])
                    {
                        string str = string.Format("流水号:{0}，名字:{1}，悟道技能ID:{2}，悟道类型:{3}{4}的等级{5}不够{6}", (object)i1, (object)uiWuDaoSkillData.Name, (object)key1, (object)UINPCData._TypeWuDaoName[key2], (object)key2, (object)dictionary[key2], (object)uiWuDaoSkillData.WuDaoLv);
                        Debug.LogError((object)str);
                        stringBuilder.AppendLine(str);
                        flag = true;
                    }
                }
            }
            if (flag)
                stringBuilder.AppendLine();
        }
        Debug.LogError((object)stringBuilder.ToString());
    }

    public int CompareTo(object obj)
    {
        if (this.Level > ((UINPCData)obj).Level)
            return 1;
        if (this.Level != ((UINPCData)obj).Level)
            return -1;
        if (this.Exp > ((UINPCData)obj).Exp)
            return 1;
        return this.Exp == ((UINPCData)obj).Exp ? 0 : -1;
    }
}
