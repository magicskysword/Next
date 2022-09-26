using System;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlIconPreviewDrawer : CtlPropertyDrawerBase
{
    public UI_ComIconPreviewDrawer MainView => (UI_ComIconPreviewDrawer)Component;
    public Func<string> IconUrlGetter { get; set; }
        
    public CtlIconPreviewDrawer(Func<string> iconUrlGetter)
    {
        IconUrlGetter = iconUrlGetter;
    }
        
    protected override GComponent OnCreateCom()
    {
        return UI_ComIconPreviewDrawer.CreateInstance();
    }

    protected override void OnRefresh()
    {
        base.OnRefresh();
        MainView.icon = IconUrlGetter();
    }

    protected override void SetDrawerEditable(bool value)
    {
            
    }
}