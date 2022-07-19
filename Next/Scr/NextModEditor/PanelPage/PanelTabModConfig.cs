using FairyGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.Extension;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.NextEditor.PanelPage
{
    public class PanelTabModConfig : PanelPageBase
    {
        public ModProject Project { get; set; }
        public CtlPropertyInspector Inspector { get; set; }

        protected override GObject OnAdd()
        {
            Inspector = new CtlPropertyInspector(UI_ComMainInspector.CreateInstance());
            var config = Project.Config;

            Inspector.AddDrawer(new CtlStringPropertyDrawer(
                "ModEditor.Main.modConfig.name".I18N(),
                str => config.Name = str,
                () => config.Name)
            );

            Inspector.AddDrawer(new CtlStringPropertyDrawer(
                "ModEditor.Main.modConfig.author".I18N(),
                str => config.Author = str,
                () => config.Author)
            );

            Inspector.AddDrawer(new CtlStringPropertyDrawer(
                "ModEditor.Main.modConfig.version".I18N(),
                str => config.Version = str,
                () => config.Version)
            );

            Inspector.AddDrawer(new CtlStringAreaPropertyDrawer(
                "ModEditor.Main.modConfig.desc".I18N(),
                str => config.Description = str,
                () => config.Description)
            );

            Inspector.Refresh();
            return Inspector.MainView;
        }

        protected override void OnRemove()
        {
            
        }

        protected override void OnOpen()
        {
            
        }
    }
}