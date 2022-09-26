using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlTextDrawer : CtlPropertyDrawerBase
{
    public CtlTextDrawer(string text)
    {
        _text = text;
    }

    private string _text;

    protected override GComponent OnCreateCom()
    {
        var label = UIPackage.CreateObject(FGUIManager.PKG_NEXT_CORE, "ComTextDrawer").asLabel;
        label.text = _text;
        return label;
    }

    protected override void SetDrawerEditable(bool value)
    {
            
    }

    public void SetText(string text)
    {
        _text = text;
        Component.text = text;
    }
}