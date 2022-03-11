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
using UnityEngine.SceneManagement;

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
        /// 对话注册事件
        /// </summary>
        private static Dictionary<string, IDialogEvent> _registerEvents = new Dictionary<string, IDialogEvent>();

        #endregion

        #region 属性

        /// <summary>
        /// 对话数据
        /// </summary>
        public static Dictionary<string, DialogEventData> DialogDataDic { get; set; } = new Dictionary<string, DialogEventData>();
        /// <summary>
        /// 对话触发器
        /// </summary>
        public static Dictionary<string, DialogTriggerData> DialogTriggerDataDic { get; set; } =
            new Dictionary<string, DialogTriggerData>();
        /// <summary>
        /// 对话临时储存角色
        /// </summary>
        public static Dictionary<string, int> TmpCharacter { get; set; } = new Dictionary<string, int>();
        
        public static DialogEnvironment CurEnv  { get; set; }
        public static Queue<DialogEventRtData> EventQueue { get; set; } = new Queue<DialogEventRtData>();

        private static ExpressionEvaluator CurEvaluator  { get; set; }
        private static bool IsRunningEvent { get; set; } = false;

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
            _registerEvents[command] = cEvent;
        }

        public static ExpressionEvaluator GetEvaluate(DialogEnvironment env)
        {
            CurEvaluator = CurEvaluator ?? new ExpressionEvaluator();
            CurEvaluator.Context = env;
            return CurEvaluator;
        }

        public static bool TryTrigger(IEnumerable<string> triggerTypes,DialogEnvironment env = null,bool triggerAll = false)
        {
            var newEnv = env ?? new DialogEnvironment();

            var triggers = DialogTriggerDataDic.Values
                .Where(triggerData => triggerTypes.Contains(triggerData.Type))
                .OrderByDescending(triggerData => triggerData.Priority);
            foreach (var trigger in triggers)
            {
                try
                {
                    if (CheckCondition(trigger.Condition,newEnv))
                    {
                        Main.LogInfo($"触发器 [{trigger.ID}] {trigger.Condition} 触发成功。");
                        if(!triggerAll)
                        {
                            // 非队列触发，直接切换事件
                            SwitchDialogEvent(trigger.TriggerEvent,newEnv);
                            return true;
                        }
                        else
                        {
                            // 队列触发，添加事件
                            StartDialogEvent(trigger.TriggerEvent,newEnv);
                        }
                    }
                }
                catch (Exception e)
                {
                    Main.LogError($"触发器 [{trigger.ID}] {trigger.Condition} 触发判断失败！请检查表达式是否正确！");
                    Main.LogError(e);
                }
            }
            
            return false;
        }
        
        public static void StartDialogEvent(string eventID,DialogEnvironment env = null)
        {
            if (!DialogDataDic.TryGetValue(eventID, out var data))
            {
                Main.LogWarning($"对话事件 {eventID} 不存在。");
                return;
            }

            StartDialogEvent(data,env);
        }
        
        public static void StartTestDialogEvent(string dialog,DialogEnvironment env = null)
        {
            var data = new DialogEventData()
            {
                ID = "next_test",
                Option = new string[0],
                Character = new Dictionary<string, int>()
            };
            
            data.Dialog = dialog.Split('\n').Where(str=>!string.IsNullOrWhiteSpace(str)).ToArray();
            StartDialogEvent(data,env);
        }

        public static bool RunDialogEventOption(DialogOptionCommand[] optionCommands,DialogEnvironment env ,ref string jumpEvent)
        {
            ClearMenu();
            var haveOption = false;
            foreach (var optionCommand in optionCommands)
            {
                // 如果存在默认跳转事件，赋值
                if (optionCommand.Option == "Default")
                {
                    jumpEvent = optionCommand.TagEvent;
                    continue;
                }
                try
                {
                    if (CheckCondition(optionCommand.Condition,env))
                    {
                        haveOption = true;
                        AddMenu(optionCommand.Option, () =>
                        {
                            if(!string.IsNullOrEmpty(optionCommand.TagEvent))
                                StartDialogEvent(optionCommand.TagEvent);
                        });
                    }
                }
                catch (Exception e)
                {
                    throw new ArgumentException($"选项 [{optionCommand.Option}]" +
                                                $" {optionCommand.Condition} 触发判断失败！请检查表达式是否正确！", e);
                }
            }

            return haveOption;
        }

        public static void RunDialogEventCommand(DialogCommand command,DialogEnvironment environment,
            Action callback = null)
        {
            if (_registerEvents.TryGetValue(command.Command, out var dialogEvent))
            {
                try
                {
                    dialogEvent.Execute(command, environment, () => callback?.Invoke());
                }
                catch (Exception e)
                {
                    throw new ArgumentNullException($"指令 {command.RawCommand} 执行错误！",e);
                }
            }
            else
            {
                throw new ArgumentNullException($"指令 {command.Command} 不存在！");
            }
        }

        public static bool CheckCondition(string condition, DialogEnvironment env)
        {
            var evaluator = GetEvaluate(env);
            return string.IsNullOrEmpty(condition) || evaluator.Evaluate<bool>(condition);
        }

        public static void TryAddTmpChar(string name, int id)
        {
            TmpCharacter[name] = id;
        }

        public static void Clear()
        {
            DialogDataDic.Clear();
            DialogTriggerDataDic.Clear();
            TmpCharacter.Clear();
            CurEvaluator = null;
            IsRunningEvent = false;
        }

        public static void CancelEvent()
        {
            IsRunningEvent = false;
            EventQueue.Clear();
        }
        
        private static void CompleteEvent()
        {
            if (EventQueue.Count > 0)
            {
                var rtEvent = EventQueue.Dequeue();
                RunDialogEvent(rtEvent,0);
            }
            else
            {
                IsRunningEvent = false;
            }
        }
        
        public static void SwitchDialogEvent(string eventId,DialogEnvironment env = null)
        {
            if (!DialogDataDic.TryGetValue(eventId, out var data))
            {
                Main.LogWarning($"对话事件 {eventId} 不存在。");
                CompleteEvent();
            }

            SwitchDialogEvent(data, env);
        }
        
        /// <summary>
        /// 事件开始入口，事件进入队列执行
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="env"></param>
        public static void StartDialogEvent(DialogEventData eventData,DialogEnvironment env = null)
        {
            var getEnv = env == null ? new DialogEnvironment() : env.Clone();
            var rtEvent = new DialogEventRtData()
            {
                BindEventData = eventData,
                BindEnv = getEnv,
            };
            if (!IsRunningEvent)
            {
                RunDialogEvent(rtEvent,0);
            }
            else
            {
                EventQueue.Enqueue(rtEvent);
            }
        }

        /// <summary>
        /// 事件开始入口，替换当前事件
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="env"></param>
        public static void SwitchDialogEvent(DialogEventData eventData,DialogEnvironment env = null)
        {
            var getEnv = env == null ? new DialogEnvironment() : env.Clone();
            var rtEvent = new DialogEventRtData()
            {
                BindEventData = eventData,
                BindEnv = getEnv,
            };
            RunDialogEvent(rtEvent,0);
        }

        #endregion

        #region 私有方法

        private static void RunDialogEvent(DialogEventRtData rtEvent, int index)
        {
            CurEnv = rtEvent.BindEnv;
            var eventData = rtEvent.BindEventData;
            IsRunningEvent = true;
            
            CurEnv.curDialogID = eventData.ID;
            CurEnv.curDialogIndex = index;

            if (index < 0 || index >= eventData.Dialog.Length)
            {
                Main.LogWarning($"对话事件 {CurEnv.curDialogID} 超出索引。");
                return;
            }

            var haveOption = false;
            var jumpEvent = string.Empty;

            var command = eventData.GetDialogCommand(index,CurEnv);
            
            if (command.IsEnd)
            {
                var optionCommands = eventData.GetOptionCommands();
                try
                {
                    haveOption = RunDialogEventOption(optionCommands, CurEnv, ref jumpEvent);
                }
                catch (Exception e)
                {
                    Main.LogError($"事件 [{CurEnv.curDialogID}] 选项判断出错，已清空选项。");
                    Main.LogError(e);
                    ClearMenu();
                    haveOption = false;
                }
            }

            try
            {
                RunDialogEventCommand(command, CurEnv, () =>
                {
                    if (!command.IsEnd)
                        RunDialogEvent(rtEvent, index + 1);
                    // 当不存在选项且有默认跳转事件时，进行跳转
                    else if (!haveOption && !string.IsNullOrEmpty(jumpEvent))
                        SwitchDialogEvent(jumpEvent);
                    else
                        CompleteEvent();
                });
            }
            catch (Exception e)
            {
                Main.LogError($"事件 [{CurEnv.curDialogID}] 第 {index} 行指令 {command.RawCommand} 执行错误");
                Main.LogError(e);
                CancelEvent();
            }
        }

        #endregion

        
    }
}