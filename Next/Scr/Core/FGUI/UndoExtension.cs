namespace SkySwordKill.Next.FGUI;

public static class UndoExtension
{
    public static void Record(this IUndoInst inst, IUndoCommand command)
    {
        command.Execute();
        inst.UndoManager?.Add(command);
    }
}