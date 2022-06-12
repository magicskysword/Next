using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.ComponentCtl;

namespace SkySwordKill.NextEditor.PanelPage
{
    public class PanelEmptyTabModConfig : PanelEmptyPage
    {
        public override void OnOpen()
        {
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
        }
    }
}