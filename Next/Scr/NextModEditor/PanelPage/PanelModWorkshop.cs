using FairyGUI;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod;

namespace SkySwordKill.NextModEditor.PanelPage;

public class PanelModWorkshop : PanelPageBase
{
    public PanelModWorkshop(string name, ModWorkshop mod) : base(name)
    {
        Mod = mod;
    }
        
    public ModWorkshop Mod;
    public CtlPropertyInspector Inspector { get; set; }
        
    protected override GObject OnAdd()
    {
        Inspector = new CtlPropertyInspector(UI_ComMainInspector.CreateInstance());
            
        Inspector.AddDrawer(new CtlStringPropertyDrawer(
            "工坊名称".I18NTodo(),
            str => Mod.ModInfo.Title = str,
            () => Mod.ModInfo.Title)
        );
            
        Inspector.AddDrawer(new CtlStringAreaPropertyDrawer(
            "工坊描述".I18NTodo(),
            str => Mod.ModInfo.Des = str,
            () => Mod.ModInfo.Des)
        );

        Inspector.Refresh();
        return Inspector.MainView;
    }

    protected override void OnOpen()
    {
            
    }

    protected override void OnRemove()
    {
            
    }


}