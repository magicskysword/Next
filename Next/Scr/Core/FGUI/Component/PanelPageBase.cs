using FairyGUI;
using SkySwordKill.NextModEditor.Mod;

namespace SkySwordKill.Next.FGUI.Component;

public abstract class PanelPageBase
{
    public PanelPageBase(string name)
    {
        Name = name;
    }
        
    public string ID { get; set; }
    public string Name { get; set; }
    public GObject Content { get; set; }

    protected abstract GObject OnAdd();
    protected abstract void OnOpen();
    protected abstract void OnRemove();

    public virtual bool OnHandleKey(InputEvent evt)
    {
        return false;
    }

    public void Create()
    {
        if (Content == null)
        {
            Content = OnAdd();
        }
    }
        
    public void Open()
    {
        OnOpen();
    }
        
    public void Close()
    {
        OnRemove();
        Content?.Dispose();
        Content = null;
    }
}