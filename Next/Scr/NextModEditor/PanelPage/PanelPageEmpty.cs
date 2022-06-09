using FairyGUI;

namespace SkySwordKill.NextEditor.PanelPage
{
    public abstract class PanelPageEmpty : PanelPageBase
    {
        public override void OnAdd()
        {
            var graph = new GGraph();
            Content = graph;
        }

        public override void OnRemove()
        {
            Content.Dispose();
        }
    }
}
