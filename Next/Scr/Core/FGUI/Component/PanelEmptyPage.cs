using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component
{
    public abstract class PanelEmptyPage : PanelPageBase
    {
        protected PanelEmptyPage(string name) : base(name)
        {
        }
        
        protected override GObject OnAdd()
        {
            var graph = new GGraph();
            return graph;
        }

        protected override void OnRemove()
        {
            
        }

        
    }
}
