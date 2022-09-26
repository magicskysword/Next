namespace SkySwordKill.Next.FGUI;

public static class UndoExtension
{
    public static void Record(this IUndoInst inst, IUndoCommand command)
    {
        command.Execute();
        if(inst.UndoManager != null)
            inst.UndoManager.Add(command);
    }
}