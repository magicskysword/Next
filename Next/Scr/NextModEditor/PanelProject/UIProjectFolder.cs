namespace SkySwordKill.NextEditor.PanelProject
{
    public class UIProjectFolder : UIProjectBase
    {
        public UIProjectFolder(string name, int layer)
        {
            Name = name;
            Layer = layer;
        }

        public override bool IsLeaf => false;
    }
}