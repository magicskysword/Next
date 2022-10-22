using System;
using System.Linq;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.Next.Mod;

namespace SkySwordKill.Next.ModGUI;

public class ProjectTreeFolderModGroup : ProjectTreeFolder
{
    public ProjectTreeFolderModGroup(ModGroup group)
    {
        ModGroup = group;
        switch (ModGroup.Type)
        {
            case ModType.Local:
                _icon = "ui://NextCore/icon_folder1";
                break;
            case ModType.Workshop:
                _icon = "ui://NextCore/icon_steam";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public ModGroup ModGroup { get; }

    public override string Name
    {
        get
        {
            if (string.IsNullOrEmpty(ModGroup.GroupName))
            {
                return $"??? [id:{ModGroup.GroupKey}]";
            }
            
            return $"{ModGroup.GroupName}";
        }
    }

    public void RefreshModConfigs()
    {
        var oldChildren = Children.ToList();
        RemoveAllChildren();
        foreach (var modConfig in ModGroup.ModConfigs)
        {
            var modItem = oldChildren.Find(x => x is ProjectTreeItemModConfig item && item.ModConfig == modConfig);
            if (modItem != null)
            {
                AddChild(modItem);
            }
            else
            {
                AddChild(new ProjectTreeItemModConfig(modConfig, ModGroup));
            }
        }
        
        
    }

    public void OnInspector(CtlPropertyInspector inspector)
    {
        var id = ModGroup.GroupKey;
        var source = ModGroup.Type == ModType.Local ? "本地Mod" : "工坊Mod";
        var path = ModGroup.ModDir.FullName;
        var desc = ModGroup.SteamModInfo.Des;

        inspector.AddDrawer(new CtlTitleDrawer(Name));
        inspector.AddDrawer(new CtlInfoDrawer($"ID", id, 16));
        inspector.AddDrawer(new CtlInfoDrawer($"来源",source, 16));
        inspector.AddDrawer(new CtlInfoLinkDrawer($"路径", path,16, path));
        inspector.AddDrawer(new CtlInfoDrawer($"包含Mod数量",ModGroup.ModConfigs.Count.ToString(), 16));
        inspector.AddDrawer(new CtlInfoDrawer($"描述", desc, 16));
    }
}