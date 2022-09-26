using System;

namespace SkySwordKill.Next.FGUI;

public class ValueChangedCommand<T> : IUndoCommand
{
    private T _oldValue;
    private T _newValue;
    private Action<T> _onValueChanged;

    public ValueChangedCommand(T oldValue, T newValue, Action<T> onValueChanged)
    {
        _oldValue = oldValue;
        _newValue = newValue;
        _onValueChanged = onValueChanged;
    }

    public void Execute()
    {
        _onValueChanged?.Invoke(_newValue);
    }

    public void Undo()
    {
        _onValueChanged?.Invoke(_oldValue);
    }
}