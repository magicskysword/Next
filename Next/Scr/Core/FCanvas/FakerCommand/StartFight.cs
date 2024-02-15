using System.Collections.Generic;
using System.Linq;
using Fungus;
using SkySwordKill.Next.Utils;

namespace SkySwordKill.Next.FCanvas.FakerCommand;

[FCommandBinder(typeof(Fungus.StartFight))]
public class StartFight : FCommand
{
    /// <summary>
    /// 设置怪物ID的方式
    /// </summary>
    public int MonsterSetType;
    /// <summary>
    /// 怪物的ID
    /// </summary>
    public string MonsterFungusID;
    /// <summary>
    /// 怪物的ID
    /// </summary>
    public int MonstarID;
    /// <summary>
    /// 给怪物加buff
    /// </summary>
    public List<string> MonstarAddBuffList = new List<string>();
    /// <summary>
    /// 给主角加buff
    /// </summary>
    public List<string> HeroAddBuffList = new List<string>();
    /// <summary>
    /// 战斗的类型
    /// </summary>
    public int FightType = 0;
    /// <summary>
    /// 设置战场背景的方式
    /// </summary>
    public int BackgroundSetType;
    /// <summary>
    /// 背景的ID
    /// </summary>
    public string BackgroundFungusID;
    /// <summary>
    /// 背景的ID
    /// </summary>
    public int BackgroundID;
    
    /// <summary>
    /// 开启战场对话
    /// </summary>
    public int FightTalk;
    
    /// <summary>
    /// 开启固定抽排
    /// </summary>
    public int FightCard;
    
    /// <summary>
    /// 是否能战前逃跑
    /// </summary>
    public int CanFpRun = 1;
    
    /// <summary>
    /// 战场音乐
    /// </summary>
    public string FightMusic = "战斗1";
    
    /// <summary>
    /// 是否开启海域移除NPC
    /// </summary>
    public bool SeaRemoveNPCFlag;
    
    /// <summary>
    /// 海域移除NPC的编号UUID
    /// </summary>
    public string SeaRemoveNPCUUID;
    
    public override void ReadCommand(Command command)
    {
        base.ReadCommand(command);
        
        var cmdStartFight = (Fungus.StartFight)command;
        MonsterSetType = (int)cmdStartFight.GetFieldValue<Fungus.StartFight.MonstarType>("MonstarSetType");
        MonsterFungusID = cmdStartFight.GetFieldValue<Fungus.IntegerVariable>("MonstarFungusID")?.Key ?? "";
        MonstarID = cmdStartFight.MonstarID;
        MonstarAddBuffList = cmdStartFight.GetFieldValue<List<StarttFightAddBuff>>("MonstarAddBuffList")
            .Select(addBuff => $"{addBuff.buffID},{addBuff.BuffNum}").ToList();
        HeroAddBuffList = cmdStartFight.GetFieldValue<List<StarttFightAddBuff>>("HeroAddBuffList")
            .Select(addBuff => $"{addBuff.buffID},{addBuff.BuffNum}").ToList();
        FightType = (int)cmdStartFight.GetFieldValue<Fungus.StartFight.FightEnumType>("FightType");
        BackgroundSetType = (int)cmdStartFight.GetFieldValue<Fungus.StartFight.MonstarType>("BackgroundSetType");
        BackgroundFungusID = cmdStartFight.GetFieldValue<Fungus.IntegerVariable>("BackgroundFungusID")?.Key ?? "";
        BackgroundID = cmdStartFight.GetFieldValue<int>("BackgroundID");
        FightTalk = cmdStartFight.GetFieldValue<int>("FightTalk");
        FightCard = cmdStartFight.GetFieldValue<int>("FightCard");
        CanFpRun = cmdStartFight.GetFieldValue<int>("CanFpRun");
        FightMusic = cmdStartFight.GetFieldValue<string>("FightMusic");
        SeaRemoveNPCFlag = cmdStartFight.GetFieldValue<bool>("SeaRemoveNPCFlag");
        SeaRemoveNPCUUID = cmdStartFight.GetFieldValue<Fungus.StringVariable>("SeaRemoveNPCUUID")?.Value ?? "";
    }

    public string GetSummary()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("开始战斗");
        if(MonsterSetType == 0)
        {
            sb.AppendLine($"敌方ID: {MonstarID}");
        }
        else
        {
            sb.AppendLine($"敌方ID: {MonsterFungusID}");
        }
        sb.AppendLine($"战斗类型: {GetFightTypeSummary(FightType)}");
        if(MonstarAddBuffList.Count > 0)
        {
            sb.Append("敌方Buff:");
            sb.AppendLine(string.Join("|", MonstarAddBuffList));
        }
        if(HeroAddBuffList.Count > 0)
        {
            sb.Append("我方Buff:");
            sb.AppendLine(string.Join("|", HeroAddBuffList));
        }
        if(BackgroundSetType == 0)
        {
            sb.AppendLine($"背景ID: {BackgroundID}");
        }
        else
        {
            sb.AppendLine($"背景ID: {BackgroundFungusID}");
        }
        if (FightTalk > 0)
        {
            sb.AppendLine($"战场对话: {(FightTalk == 1 ? "开启" : "关闭")}");
        }
        if (FightCard > 0)
        {
            sb.AppendLine($"固定抽排: {(FightCard == 1 ? "开启" : "关闭")}");
        }
        sb.AppendLine($"战前逃跑: {(CanFpRun == 1 ? "允许" : "禁止")}");
        sb.AppendLine($"战场音乐: {FightMusic}");
        if (SeaRemoveNPCFlag)
        {
            sb.AppendLine($"海域移除NPC: {SeaRemoveNPCUUID}");
        }
        
        return sb.ToString();
    }

    // Normal = 1,
    // XingMo = 2,
    // DuJie = 3,
    // LeiTai = 4,
    // HuanJin = 5,
    // BossZhan = 6,
    // DiFangTaoLi = 7,
    // QieCuo = 8,
    // XinShouYinDao = 9,
    // DouFa = 10, // 0x0000000A
    // JieDan = 11, // 0x0000000B
    // ZhangLaoZhan = 12, // 0x0000000C
    // BuShaDuiShou = 13, // 0x0000000D
    // JieYing = 14, // 0x0000000E
    // ZhuJi = 15, // 0x0000000F
    // 古树根须 = 16, // 0x00000010
    // 生死比试 = 17, // 0x00000011
    // HuaShen = 18, // 0x00000012
    // FeiSheng = 19, // 0x00000013
    // 无装备无丹药擂台 = 20, // 0x00000014
    // 天劫秘术领悟 = 21, // 0x00000015
    // 双方逃离 = 22, // 0x00000016
    // 煅体 = 23, // 0x00000017
    // 玩家不死亡 = 24, // 0x00000018
    public static string GetFightTypeSummary(int fightType)
    {
        switch (fightType)
        {
            case 1:
                return "普通战斗";
            case 2:
                return "心魔";
            case 3:
                return "渡劫";
            case 4:
                return "擂台";
            case 5:
                return "幻境";
            case 6:
                return "Boss战";
            case 7:
                return "敌方逃离";
            case 8:
                return "切磋";
            case 9:
                return "新手引导";
            case 10:
                return "斗法";
            case 11:
                return "结丹";
            case 12:
                return "长老战";
            case 13:
                return "不杀对手";
            case 14:
                return "结婴";
            case 15:
                return "筑基";
            case 16:
                return "古树根须";
            case 17:
                return "生死比试";
            case 18:
                return "化神";
            case 19:
                return "飞升";
            case 20:
                return "无装备无丹药擂台";
            case 21:
                return "天劫秘术领悟";
            case 22:
                return "双方逃离";
            case 23:
                return "煅体";
            case 24:
                return "玩家不死亡";
            default:
                return "未知";
        }
    }
}