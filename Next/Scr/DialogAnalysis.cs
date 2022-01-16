using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogTrigger;
using UnityEngine.Events;

namespace SkySwordKill.Next
{
    public static partial class DialogAnalysis
    {
        #region 字段

        public static Lazy<MethodInfo> addOptionMethod = new Lazy<MethodInfo>(() =>
        {
            var method = typeof(Fungus.MenuDialog).GetMethod("AddOption", 
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new Type[]
                {
                    typeof(string),
                    typeof(bool),
                    typeof(bool),
                    typeof(UnityAction)
                },
                null);
            return method;
        });

        /// <summary>
        /// 对话数据
        /// </summary>
        public static Dictionary<string, DialogEventData> dialogDataDic = new Dictionary<string, DialogEventData>();
        /// <summary>
        /// 对话注册事件
        /// </summary>
        public static Dictionary<string, IDialogEvent> dialogEventDic = new Dictionary<string, IDialogEvent>();
        /// <summary>
        /// 对话临时储存角色
        /// </summary>
        public static Dictionary<string, int> tmpCharacter = new Dictionary<string, int>();
        /// <summary>
        /// 对话触发器
        /// </summary>
        public static Dictionary<string, DialogTriggerData> dialogTriggerDataDic =
            new Dictionary<string, DialogTriggerData>();

        public static string curDialogID;
        public static int curDialogIndex = 0;
        public static DialogEnvironment curEnv;
        
        private static ExpressionEvaluator curEvaluator;

        #endregion

        #region 属性



        #endregion

        #region 回调方法



        #endregion

        #region 公共方法

        public static void Init()
        {

            foreach (var types in AppDomain.CurrentDomain.GetAssemblies()
                .Select(assembly => assembly.GetTypes()))
            {
                foreach (var type in types)
                {
                    // 注册触发器
                    if(type.Namespace == "SkySwordKill.Next.DialogTrigger")
                    {
                        Harmony.CreateAndPatchAll(type);
                        continue;
                    }
                    // 注册事件指令
                    if (typeof(IDialogEvent).IsAssignableFrom(type))
                    {
                        foreach (var attribute in type.GetCustomAttributes<DialogEventAttribute>())
                        {
                            var command = attribute.registerCommand;
                            RegisterCommand(command,Activator.CreateInstance(type) as IDialogEvent);
                        }
                    }
                    
                }
            }
        }

        public static void RegisterCommand(string command, IDialogEvent cEvent)
        {
            dialogEventDic[command] = cEvent;
        }

        public static ExpressionEvaluator GetEvaluate(DialogEnvironment env)
        {
            curEvaluator = curEvaluator ?? new ExpressionEvaluator();
            curEvaluator.Context = env;
            return curEvaluator;
        }

        public static bool TryTrigger(IEnumerable<string> triggerTypes,DialogEnvironment env = null)
        {
            var newEnv = env ?? new DialogEnvironment();

            var triggers = dialogTriggerDataDic.Values.Where(triggerData => triggerTypes.Contains(triggerData.type));
            foreach (var trigger in triggers)
            {
                try
                {
                    if (CheckCondition(trigger.condition,newEnv))
                    {
                        Main.LogInfo($"触发器 [{trigger.id}] {trigger.condition} 触发成功。");
                        StartDialogEvent(trigger.triggerEvent,newEnv);
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Main.LogError($"触发器 [{trigger.id}] {trigger.condition} 触发判断失败！请检查表达式是否正确！");
                    Main.LogError(e);
                }
            }
            
            return false;
        }
        
        public static void StartDialogEvent(string eventID,DialogEnvironment env = null)
        {
            curEnv = env ?? new DialogEnvironment();
            RunDialogEvent(eventID,0);
        }

        public static void StartTestDialogEvent(string dialog)
        {
            if (!dialogDataDic.TryGetValue("next_test", out DialogEventData data))
            {
                data = new DialogEventData
                {
                    id = "next_test",
                    option = new string[0],
                    character = new Dictionary<string, int>()
                };
                dialogDataDic["next_test"] = data;
            }
            data.dialog = dialog.Split('\n').Where(str=>!string.IsNullOrWhiteSpace(str)).ToArray();
            StartDialogEvent("next_test");
        }

        public static void RunNextDialogEvent()
        {
            RunDialogEvent(curDialogID,curDialogIndex + 1);
        }

        public static void RunDialogEvent(string eventID, int index)
        {
            curDialogID = eventID;
            curDialogIndex = index;
            if (!dialogDataDic.TryGetValue(eventID, out var data))
            {
                Main.LogWarning($"对话事件 {eventID} 不存在。");
                return;
            }

            if (index < 0 || index >= data.dialog.Length)
            {
                Main.LogWarning($"对话事件 {eventID} 超出索引。");
                return;
            }

            var haveOption = false;
            var jumpEvent = string.Empty;

            var command = data.GetDialogCommand(index,curEnv);
            
            if (command.isEnd)
            {
                ClearMenu();
                var optionCommands = data.GetOptionCommands();
                
                foreach (var optionCommand in optionCommands)
                {
                    if (optionCommand.option == "Default")
                    {
                        jumpEvent = optionCommand.tagEvent;
                        continue;
                    }
                    try
                    {
                        if (CheckCondition(optionCommand.condition,curEnv))
                        {
                            haveOption = true;
                            AddMenu(optionCommand.option, () =>
                            {
                                if(!string.IsNullOrEmpty(optionCommand.tagEvent))
                                    StartDialogEvent(optionCommand.tagEvent);
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        Main.LogError($"事件 [{eventID}] 选项 [{optionCommand.option}]" +
                                      $" {optionCommand.condition} 触发判断失败！请检查表达式是否正确！");
                        Main.LogError(e);
                    }
                }
            }

            if (dialogEventDic.TryGetValue(command.command, out var dialogEvent))
            {
                try
                {
                    dialogEvent.Execute(command,curEnv, () =>
                    {
                        if(!command.isEnd)
                            RunNextDialogEvent();
                        else if(!haveOption && !string.IsNullOrEmpty(jumpEvent))
                            StartDialogEvent(jumpEvent);
                    });
                }
                catch (Exception e)
                {
                    Main.LogError(e);
                    Main.LogError($"事件 [{eventID}] 第 {index} 行指令 {command.rawCommand} 执行错误");
                }
            }
            else
            {
                Main.LogError($"指令 {command.command} 不存在！");
                Main.LogError($"事件 [{eventID}] 第 {index} 行指令 {command.rawCommand} 执行错误");
            }
        }

        public static bool CheckCondition(string condition, DialogEnvironment env)
        {
            var evaluator = GetEvaluate(env);
            return string.IsNullOrEmpty(condition) || evaluator.Evaluate<bool>(condition);
        }

        public static void TryAddTmpChar(string name, int id)
        {
            tmpCharacter[name] = id;
        }

        #endregion

        #region 私有方法



        #endregion

        
    }
}