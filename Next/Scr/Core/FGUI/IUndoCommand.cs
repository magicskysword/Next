namespace SkySwordKill.Next.FGUI;

public interface IUndoCommand
{ 
    void Execute();
    void Undo();
}