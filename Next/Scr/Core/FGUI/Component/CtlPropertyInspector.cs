using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlPropertyInspector : CtlInspectorBase
{
    public CtlPropertyInspector(UI_ComMainInspector uiCom)
    {
        MainView = uiCom;
    }
        
    public UI_ComMainInspector MainView;
    
    protected override GList _drawerGList => MainView.m_list;
}