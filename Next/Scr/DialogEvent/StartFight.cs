using System;
using GUIPackage;
using HarmonyLib;
using SkySwordKill.Next.DialogTrigger;
using UnityEngine;
using UnityEngine.SceneManagement;
using YSGame;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("StartFight")]
    public class StartFight : IDialogEvent
    {
        public static string[] FightTags;

        private static string lastScene = string.Empty;
        
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            try
            {
                var id = NPCEx.NPCIDToNew(command.GetInt(0));
                // 0无法逃跑 1可以逃跑
                var canRun = command.GetInt(1);
                var fightType = (Fungus.StartFight.FightEnumType)command.GetInt(2);
            
                var background = command.GetInt(3);
                var music = command.GetStr(4);
            
                var playerBuffStr = command.GetStr(5);
                var enemyBuffStr = command.GetStr(6);
            
                var victoryEvent = command.GetStr(7);
                var defeatEvent = command.GetStr(8);
                var tags = command.GetStr(9);
                
                SetEventFight(victoryEvent,defeatEvent,tags.Split(','));

                foreach (var buffStr in playerBuffStr.Split('|'))
                {
                    if(string.IsNullOrEmpty(buffStr))
                        continue;
                    
                    var buffInfo = buffStr.Split(',');
                    var buffId = int.Parse(buffInfo[0]);
                    var buffNum = int.Parse(buffInfo[1]);
                    if (!Tools.instance.monstarMag.HeroAddBuff.ContainsKey(buffId))
                    {
                        Tools.instance.monstarMag.HeroAddBuff.Add(buffId, buffNum);
                    }
                }
            
                foreach (var buffStr in enemyBuffStr.Split('|'))
                {
                    if(string.IsNullOrEmpty(buffStr))
                        continue;
                    var buffInfo = buffStr.Split(',');
                    var buffId = int.Parse(buffInfo[0]);
                    var buffNum = int.Parse(buffInfo[1]);
                    if (!Tools.instance.monstarMag.monstarAddBuff.ContainsKey(buffId))
                    {
                        Tools.instance.monstarMag.monstarAddBuff.Add(buffId, buffNum);
                    }
                }
            
                MusicMag.instance.playMusic(music);
                Tools.instance.monstarMag.FightImageID = background;
                Tools.instance.CanFpRun = canRun;
                Tools.instance.monstarMag.FightType = fightType;
                Tools.instance.startFight(id);
            }
            catch (Exception)
            {
                ResetEventFight();
                throw;
            }
            DialogAnalysis.CancelEvent();
        }

        public static void SetEventFight(string victory, string defeat, string[] tags)
        {
            FightTags = tags;
            DialogAnalysis.SetStrOld("Fight","VictoryEvent",victory);
            DialogAnalysis.SetStrOld("Fight","DefeatEvent",defeat);
            DialogAnalysis.SetIntOld("Fight", "IsVictory", 0);
            DialogAnalysis.SetIntOld("Fight", "IsEventFight", 1);
        }

        public static void ResetEventFight()
        {
            if(Tools.instance.getPlayer() == null)
                return;
            
            FightTags = null;
            DialogAnalysis.SetStrOld("Fight","VictoryEvent","");
            DialogAnalysis.SetStrOld("Fight","DefeatEvent","");
            DialogAnalysis.SetIntOld("Fight", "IsVictory", 0);
            DialogAnalysis.SetIntOld("Fight", "IsEventFight", 0);
        }

        [HarmonyPatch(typeof(Fight.FightResultMag),"ShowVictory")]
        public class VictoryPatch
        {
            [HarmonyPrefix]
            public static void Prefix()
            {
                DialogAnalysis.SetIntOld("Fight", "IsVictory", 1);
            }
        }

        [HarmonyPatch(typeof(UIDeath),"BackToMainMenu")]
        public class DeathPatch
        {
            [HarmonyPrefix]
            public static void Prefix()
            {
                ResetEventFight();
            }
        }
        
        [HarmonyPatch(typeof(UI_Manager),"Update")]
        public class AfterBattleDialogPatch
        {
            [HarmonyPrefix]
            public static void Prefix()
            {
                if(Tools.instance.getPlayer() == null)
                    return;
                
                string sceneName = SceneManager.GetActiveScene().name;
                var IsEventFight = DialogAnalysis.GetIntOld("Fight", "IsEventFight") == 1;
                var IsInBattle = sceneName == "YSNewFight";
                if(Tools.instance.isNeedSetTalk && IsEventFight)
                {
                    if (Tools.instance.loadSceneType == 0)
                    {
                        // 使用NextScene跳转 跳转到战斗场景
                        if (sceneName.Equals(Tools.instance.ohtherSceneName) && IsInBattle)
                        {
                            OnFightStart.Trigger();
                        }
                    }
                    else 
                    {
                        // 直接跳转 跳转到大地图
                        if (sceneName.Equals(Tools.jumpToName))
                        {
                            Tools.instance.CanShowFightUI = 0;
                            
                            var VictoryEvent = DialogAnalysis.GetStrOld("Fight", "VictoryEvent");
                            var DefeatEvent = DialogAnalysis.GetStrOld("Fight", "DefeatEvent");
                            var IsVictory = DialogAnalysis.GetIntOld("Fight", "IsVictory") == 1;

                            if (IsVictory && !string.IsNullOrEmpty(VictoryEvent))
                            {
                                DialogAnalysis.StartDialogEvent(VictoryEvent);
                            }
                            else if (!IsVictory && !string.IsNullOrEmpty(DefeatEvent))
                            {
                                DialogAnalysis.StartDialogEvent(DefeatEvent);
                            }
                            ResetEventFight();
                        }
                    }
                }
            }
        }
        
        [HarmonyPatch(typeof(RoundManager),"Awake")]
        public class BackgroundPatch
        {
            [HarmonyPostfix]
            public static void Postfix(RoundManager __instance)
            {
                if (!RoundManager.TuPoTypeList.Contains(Tools.instance.monstarMag.FightType))
                {
                    if (Tools.instance.monstarMag.FightImageID != 0)
                    {
                        if (Main.Instance.resourcesManager.TryGetAsset($"Assets/Fightimage/{Tools.instance.monstarMag.FightImageID}.png", out var asset))
                        {
                            if (asset is Texture2D texture)
                            {
                                Sprite sprite = Main.Instance.resourcesManager.GetSpriteCache(texture);
                                __instance.BackGroud.sprite = sprite;
                            }
                        }
                    }
                }
            }
        }
    }
}