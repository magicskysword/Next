using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlTitleDrawer : CtlPropertyDrawerBase
{
    public CtlTitleDrawer(string title)
    {
        _title = title;
    }
    private string _title;


    protected override GComponent OnCreateCom()
    {
        var drawer = UI_ComTitleDrawer.CreateInstance();
        drawer.title = _title;
        return drawer;
    }

    protected override void SetDrawerEditable(bool value)
    {
            
    }
}