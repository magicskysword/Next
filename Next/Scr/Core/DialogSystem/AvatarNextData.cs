using System.Collections.Generic;

namespace SkySwordKill.Next.DialogSystem;

public class AvatarNextData
{
    public DataGroup<int> IntGroup { get; set; } = new DataGroup<int>();
    public DataGroup<string> StrGroup { get; set; } = new DataGroup<string>();
    public Dictionary<string, TriggerState> TriggerStates { get; set; } = new Dictionary<string, TriggerState>();

    public TriggerState GetTriggerState(string triggerID, bool createNew = true, bool triggerDefault = true)
    {
        if (TriggerStates.TryGetValue(triggerID, out var state))
        {
            return state;
        }
        else if (createNew)
        {
            state = new TriggerState();
            TriggerStates.Add(triggerID, state);
            state.Enabled = triggerDefault;
            return state;
        }
        else
        {
            return null;
        }
    }
    
    /// <summary>
    /// 设置触发器开关
    /// </summary>
    /// <param name="triggerID"></param>
    /// <param name="on"></param>
    public void SetTrigger(string triggerID, bool on)
    {
        var state = GetTriggerState(triggerID);
        state.Enabled = on;
    }
    
    /// <summary>
    /// 修改触发器触发次数
    /// </summary>
    /// <param name="triggerID"></param>
    /// <param name="delta"></param>
    public void ChangeTriggerCount(string triggerID, int delta)
    {
        var state = GetTriggerState(triggerID);
        state.Count += delta;
    }
}