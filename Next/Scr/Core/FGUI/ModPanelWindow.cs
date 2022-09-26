namespace SkySwordKill.Next.FGUI;

public class ModPanelWindow : FGUIWindowBase
{
    public ModPanelWindow() : base("NextCore", "ModMainPanel")
    {
    }

    protected override void OnInit()
    {
        base.OnInit();

        frame.asLabel.title = $"Next  v{Main.MOD_VERSION}";
            
        MakeFullScreenAndCenter(0.8f);
    }
}