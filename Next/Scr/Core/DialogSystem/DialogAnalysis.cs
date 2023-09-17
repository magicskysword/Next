using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SkySwordKill.Next.DialogEvent;
using UnityEngine.Events;

namespace SkySwordKill.Next.DialogSystem;

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
    private static Dictionary<string, IDialogEnvQuery> _registerEnvQueries = new Dictionary<string, IDialogEnvQuery>();
        
    public static event Action OnDialogComplete;

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
    public static bool IsRunningEvent { get;private set; } = false;
    public static bool IsBreakTalk { get;private set; } = false;

    #endregion

    #region 回调方法

    public static void OnEnterWorld()
    {
        CancelEvent();
    }

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
                        var command = attribute.RegisterCommand;
                        RegisterCommand(command,Activator.CreateInstance(type) as IDialogEvent);
                    }
                }
                else if (typeof(IDialogEnvQuery).IsAssignableFrom(type))
                {
                    foreach (var attribute in type.GetCustomAttributes<DialogEnvQueryAttribute>())
                    {
                        var command = attribute.RegisterMethod;
                        RegisterEnvQuery(command,Activator.CreateInstance(type) as IDialogEnvQuery);
                    }
                }

            }
        }
    }

    public static void RegisterCommand(string command, IDialogEvent cEvent)
    {
        _registerEvents[command] = cEvent;
    }
        
    private static void RegisterEnvQuery(string method, IDialogEnvQuery cQuery)
    {
        _registerEnvQueries[method] = cQuery;
    }

    private static void Evaluator_PreEvaluateFunction(object sender, FunctionPreEvaluationEventArg e)
    {
        var ext = GetEnvQuery(e.Name);
        DialogEnvironment env;
        if (e.This == null)
        {
            env = e.Evaluator.Context as DialogEnvironment;
        }
        else
        {
            env = e.This as DialogEnvironment;
        }
        
        if (ext != null && env != null)
        {
            e.Value = ext.Execute(new DialogEnvQueryContext(env, e.EvaluateArgs()));
            e.FunctionReturnedValue = true;
        }
    }
        
    public static ExpressionEvaluator GetEvaluate(DialogEnvironment env)
    {
        CurEvaluator = CurEvaluator ?? new ExpressionEvaluator();
        CurEvaluator.Context = env;
        CurEvaluator.PreEvaluateFunction += Evaluator_PreEvaluateFunction;
        return CurEvaluator;
    }
    
    public static bool TryTriggerByID(string triggerID,DialogEnvironment env = null)
    {
        var newEnv = env ?? new DialogEnvironment();
        if (DialogTriggerDataDic.TryGetValue(triggerID, out var triggerData))
        {
            try
            {
                if (!IsTriggerOn(triggerData))
                {
                    return false;
                }
                
                if (CheckCondition(triggerData.Condition,newEnv))
                {
                    Main.LogInfo($"触发器 [{triggerID}] {triggerData.Condition} 触发成功。");
                    if (triggerData.Once)
                    {
                        AvatarNextData.SetTrigger(triggerID, false);
                    }
                    AvatarNextData.ChangeTriggerCount(triggerID, 1);
                    SwitchDialogEvent(triggerID, newEnv);
                    return true;
                }
            }
            catch (Exception e)
            {
                Main.LogError($"触发器 [{triggerID}] {triggerData.Condition} 触发失败。");
                Main.LogError(e);
            }
        }
        else
        {
            Main.LogError($"触发器 [{triggerID}] 不存在。");
        }
        return false;
    }

    public static bool TryTrigger(IEnumerable<string> triggerTypes,DialogEnvironment env = null,bool triggerAll = false)
    {
        var newEnv = env ?? new DialogEnvironment();

        var triggers = DialogTriggerDataDic.Values
            .Where(triggerData => triggerTypes.Contains(triggerData.Type))
            .OrderByDescending(triggerData => triggerData.Priority);
            
        var triggerSuccess = false;
        foreach (var trigger in triggers)
        {
            try
            {
                // 如果触发器关闭则不参与判断
                if(!IsTriggerOn(trigger))
                    continue;
                
                if (CheckCondition(trigger.Condition,newEnv))
                {
                    Main.LogInfo($"触发器 [{trigger.ID}] {trigger.Condition} 触发成功。");
                    if (trigger.Once)
                    {
                        AvatarNextData.SetTrigger(trigger.ID, false);
                    }
                    AvatarNextData.ChangeTriggerCount(trigger.ID, 1);
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
                        triggerSuccess = true;
                    }
                }
            }
            catch (Exception e)
            {
                Main.LogError($"触发器 [{trigger.ID}] {trigger.Condition} 触发判断失败！请检查表达式是否正确！");
                Main.LogError(e);
            }
        }
        return triggerSuccess;
    }

    public static bool IsTriggerOn(DialogTriggerData triggerData)
    {
        var triggerState = AvatarNextData.GetTriggerState(triggerData.ID, false);
        if(triggerState == null)
            return triggerData.Default;
        
        return triggerState.Enabled;
    }

    public static void StartDialogEvent(string eventID, DialogEnvironment env = null)
    {
        if (!DialogDataDic.TryGetValue(eventID, out var data))
        {
            Main.LogWarning($"对话事件 {eventID} 不存在。");
            return;
        }

        StartDialogEvent(data,env);
    }

    public static void StartTestDialogEvent(string dialog, DialogEnvironment env = null)
    {
        var data = new DialogEventData()
        {
            ID = "next_test",
            Option = new string[0],
            Character = new Dictionary<string, int>()
        };

        data.Dialog = dialog.Trim().Split('\n').Where(str=>!string.IsNullOrWhiteSpace(str)).ToArray();
        StartDialogEvent(data,env);
    }

    /// <summary>
    /// 显示对话跳转选项
    /// </summary>
    /// <param name="optionCommands"></param>
    /// <param name="env"></param>
    /// <param name="jumpEvent"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
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
                            SwitchDialogEvent(optionCommand.TagEvent);
                        else
                            CompleteEvent();
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

    /// <summary>
    /// 运行剧情指令
    /// </summary>
    /// <param name="command"></param>
    /// <param name="environment"></param>
    /// <param name="callback"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void RunDialogEventCommand(DialogCommand command,DialogEnvironment environment,
        Action callback = null)
    {
        if (_registerEvents.TryGetValue(command.Command, out var dialogEvent))
        {
            try
            {
                dialogEvent.Execute(command, environment, () =>
                {
                    callback?.Invoke();
                });
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool StringIsNullOrTrue(string str)
    {
        return string.IsNullOrWhiteSpace(str) || str.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    public static bool CheckCondition(string condition, DialogEnvironment env = null)
    {
        var evaluator = GetEvaluate(env);
        return string.IsNullOrWhiteSpace(condition) || evaluator.Evaluate<bool>(condition);
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
        EventQueue.Clear();
        OnDialogComplete = null;
        CompleteEvent();
    }

    public static void CompleteEvent()
    {
        if (EventQueue.Count > 0)
        {
            var rtEvent = EventQueue.Dequeue();
            RunDialogEvent(rtEvent,0);
        }
        else
        {
            IsRunningEvent = false;
            OnDialogComplete?.Invoke();
            OnDialogComplete = null;
            if(IsBreakTalk)
            {
                Tools.instance.isNeedSetTalk = IsBreakTalk;
                IsBreakTalk = false;
            }
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
        if (Tools.instance.isNeedSetTalk)
        {
            Tools.instance.isNeedSetTalk = false;
            IsBreakTalk = true;
        }

        CurEnv.curDialogID = eventData.ID;
        CurEnv.curDialogIndex = index;

        if (index < 0 || index >= eventData.Dialog.Length)
        {
            Main.LogWarning($"对话事件 {CurEnv.curDialogID} 超出索引。");
            CompleteEvent();
            return;
        }

        var haveOption = false;
        var jumpEvent = string.Empty;
        DialogCommand command;
        try
        {
            command = eventData.GetDialogCommand(index,CurEnv);
        }
        catch (Exception e)
        {
            Main.LogError($"事件 [{CurEnv.curDialogID}] 第 {index} 行指令 {eventData.Dialog[index]} 解析错误");
            Main.LogError(e);
            CancelEvent();
            return;
        }

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
                CancelEvent();
                return;
            }
        }

        try
        {
            if (haveOption)
            {
                // 有选项不执行回调
                RunDialogEventCommand(command, CurEnv);
            }
            else
            {
                RunDialogEventCommand(command, CurEnv, () =>
                {
                    if (!command.IsEnd)
                        RunDialogEvent(rtEvent, index + 1);
                    // 有默认跳转事件时，进行跳转
                    else if (!string.IsNullOrEmpty(jumpEvent))
                        SwitchDialogEvent(jumpEvent);
                    else
                        CompleteEvent();
                });
            }


        }
        catch (Exception e)
        {
            Main.LogError($"事件 [{CurEnv.curDialogID}] 第 {index} 行指令 {command.RawCommand} 执行错误");
            Main.LogError(e);
            CancelEvent();
            return;
        }
    }

    #endregion
}