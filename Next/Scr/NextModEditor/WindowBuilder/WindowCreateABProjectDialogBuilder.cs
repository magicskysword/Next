using SkySwordKill.Next.NextModEditor.Window;

namespace SkySwordKill.Next.NextModEditor.WindowBuilder;

public class WindowCreateABProjectDialogBuilder
{
    public WindowCreateABProjectDialog Build()
    {
        var window = new WindowCreateABProjectDialog();
        window.modal = true;
        return window;
    }
}