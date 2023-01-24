using System.Collections.Generic;

namespace SkySwordKill.Next.FGUI;

/// <summary>
/// 序列命令，用于将多个命令组合成一个命令
/// 在记录命令后，就不能再添加命令了
/// </summary>
public class SequenceCommand : IUndoCommand
{
    public SequenceCommand(params IUndoCommand[] commands)
    {
        _commands.AddRange(commands);
    }
    
    private List<IUndoCommand> _commands = new List<IUndoCommand>();
    public IReadOnlyList<IUndoCommand> Commands => _commands;
    
    public void AddCommand(IUndoCommand command)
    {
        _commands.Add(command);
    }
    
    public void AddCommands(IEnumerable<IUndoCommand> commands)
    {
        _commands.AddRange(commands);
    }
    
    public void AddCommands(params IUndoCommand[] commands)
    {
        _commands.AddRange(commands);
    }

    public void Execute()
    {
        for (var index = 0; index < Commands.Count; index++)
        {
            var command = Commands[index];
            command.Execute();
        }
    }

    public void Undo()
    {
        for (int i = Commands.Count - 1; i >= 0; i--)
        {
            var command = Commands[i];
            command.Undo();
        }
    }
}