using System;

namespace SkySwordKill.Next.FGUI;

public class CommonUndoCommand : IUndoCommand
{
    private Action _execute;
    private Action _undo;
        
    public CommonUndoCommand(Action execute, Action undo)
    {
        _execute = execute;
        _undo = undo;
    }
        
    public void Execute()
    {
        _execute?.Invoke();
    }

    public void Undo()
    {
        _undo?.Invoke();
    }
}