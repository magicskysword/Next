using System.IO;
using SkySwordKill.Next;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextModEditor.Mod;

namespace SkySwordKill.NextModEditor.PanelProject;

public class ProjectListCreateWorkshopEmpty : ProjectListCreateWorkshopItem
{
    public override string Name => "空白项目";

    public string ModDir { get; set; } = "新工程";

    public string ModName { get; set; } = "空白Mod";
        
    public override void OnInspect()
    {
        Inspector.AddDrawer(new CtlTitleDrawer("空白工程".I18NTodo()));
            
        CtlTextDrawer modPathDrawer = new CtlTextDrawer(string.Format("项目路径：{0}".I18NTodo(), GetModPath()));
        Inspector.AddDrawer(modPathDrawer);
            
        Inspector.AddDrawer(new CtlStringPropertyDrawer("项目目录".I18NTodo(),
            str =>
            {
                ModDir = str;
                modPathDrawer.SetText(string.Format("项目路径：{0}".I18NTodo(), GetModPath()));
            },
            () => ModDir));
            
        Inspector.AddDrawer(new CtlStringPropertyDrawer("项目名称".I18NTodo(),
            str => ModName = str,
            () => ModName));
    }
        
    public override ModWorkshop OnCreateWorkshop()
    {
        if (string.IsNullOrEmpty(ModDir))
        {
            throw new CreateWorkshopException("目标目录不能为空！");
        }

        if (Directory.Exists(GetModPath()))
        {
            throw new CreateWorkshopException("目标目录已存在！");
        }

        Directory.CreateDirectory(GetModPath());

        var mod = ModEditorManager.I.CreateWorkshop(GetModPath(), ModName);
        mod.CreateProject("mod新项目");
        ModEditorManager.I.SaveWorkshop(mod);
            
        return ModEditorManager.I.LoadWorkshop(GetModPath());
    }

    private string GetModPath()
    {
        var path = $"{Main.PathLocalModsDir.Value}/{ModDir}".Replace("\\", "/");

        return path;
    }

        
}