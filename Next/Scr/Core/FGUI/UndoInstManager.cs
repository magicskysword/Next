using System;
using System.Collections.Generic;

namespace SkySwordKill.Next.FGUI;

public class UndoInstManager
{
    private List<IUndoCommand> UndoBuffer = new List<IUndoCommand>();
    private List<IUndoCommand> RedoBuffer = new List<IUndoCommand>();
        
    public int Capacity { get; set; }
    public bool CanUndo => UndoBuffer.Count > 0;
    public bool CanRedo => RedoBuffer.Count > 0;

    public event Action OnChanged;
        
    public UndoInstManager(int capacity = 50)
    {
        Capacity = capacity;
    }
        
    public void Add(IUndoCommand command)
    {
        if (UndoBuffer.Count >= Capacity)
        {
            UndoBuffer.RemoveAt(0);
        }
        UndoBuffer.Add(command);
        RedoBuffer.Clear();
        OnChanged?.Invoke();
    }
        
    /// <summary>
    /// 返回Undo的命令
    /// </summary>
    /// <returns></returns>
    public IUndoCommand Undo()
    {
        IUndoCommand command = null;
        if (UndoBuffer.Count > 0)
        {
            command = UndoBuffer[UndoBuffer.Count - 1];
            command.Undo();
            UndoBuffer.RemoveAt(UndoBuffer.Count - 1);
            RedoBuffer.Add(command);
        }
        OnChanged?.Invoke();
        return command;
    }
        
    /// <summary>
    /// 返回Redo的命令
    /// </summary>
    public void Redo()
    {
        if (RedoBuffer.Count > 0)
        {
            var command = RedoBuffer[RedoBuffer.Count - 1];
            command.Execute();
            RedoBuffer.RemoveAt(RedoBuffer.Count - 1);
            UndoBuffer.Add(command);
        }
        OnChanged?.Invoke();
    }
}