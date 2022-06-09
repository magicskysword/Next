using FairyGUI;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.NextEditor.PanelPage
{
    public abstract class PanelPageBase
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public ModProject Project { get; set; }
        public UI_ComMainInspector Inspector { get; set; }
        public GObject Content { get; set; }
    
        public abstract void OnAdd();
        public abstract void OnOpen();
        public abstract void OnRemove();
    }
}