using SkySwordKill.Next.Extension;
using SkySwordKill.NextEditor.Drawer;

namespace SkySwordKill.NextEditor.PanelPage
{
    public class PanelTabModConfig : PanelPageEmpty
    {
        public override void OnOpen()
        {
            var config = Project.Config;

            Inspector.AddDrawer(new ModStringPropertyDrawer(
                "ModEditor.Main.modConfig.name".I18N(),
                str => config.Name = str,
                () => config.Name)
            );

            Inspector.AddDrawer(new ModStringPropertyDrawer(
                "ModEditor.Main.modConfig.author".I18N(),
                str => config.Author = str,
                () => config.Author)
            );

            Inspector.AddDrawer(new ModStringPropertyDrawer(
                "ModEditor.Main.modConfig.version".I18N(),
                str => config.Version = str,
                () => config.Version)
            );

            Inspector.AddDrawer(new ModStringAreaPropertyDrawer(
                "ModEditor.Main.modConfig.desc".I18N(),
                str => config.Description = str,
                () => config.Description)
            );

            Inspector.Refresh();
        }
    }
}