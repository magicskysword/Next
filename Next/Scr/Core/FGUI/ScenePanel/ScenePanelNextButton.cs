using FairyGUI;
using SkySwordKill.Next.FGUI;
using SkySwordKill.Next.ModGUI;

namespace Next.Scr.Core.FGUI.ScenePanel;

public class ScenePanelNextButton : FGUIScenePanelBase
{
    public ScenePanelNextButton() : base(FGUIManager.PKG_NEXT_CORE, "BtnNext")
    {
    }

    public GButton Button => contentPane.asButton;

    protected override void OnInit()
    {
        base.OnInit();
        
        Button.onClick.Add(OnClickButton);
    }

    protected override void DoShowAnimation()
    {
        var root = GRoot.inst;
        Button.SetXY(root.width - Button.width , 0);
        Button.AddRelation(root, RelationType.Right_Right);
        Button.AddRelation(root, RelationType.Top_Top);
        
        base.DoShowAnimation();
    }
    
    private void OnClickButton(EventContext context)
    {
        var window = new ModMainWindow();
        window.Show();
    }
}