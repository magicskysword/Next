using System.Collections.Generic;
using SkySwordKill.Next.NextModEditor.Window;
using SkySwordKill.NextModEditor.PanelProject;

namespace SkySwordKill.Next.NextModEditor.WindowBuilder;

public class WindowCreateWorkshopModDialogBuilder
{
    public string Title { get; set; }
    public OnSelectMod OnSelectMod { get; set; }
    public IEnumerable<ProjectListCreateWorkshopItem> ItemList { get; set; }

    public WindowCreateWorkshopModDialog Build()
    {
        var window = new WindowCreateWorkshopModDialog();
        window.Title = Title;
        window.OnConfirm = OnSelectMod;

        if (ItemList != null)
        {
            window.ProjectList.AddRange(ItemList);
        }
        
        window.modal = true;
        return window;
    }
    
    public WindowCreateWorkshopModDialogBuilder SetTitle(string title)
    {
        Title = title;
        return this;
    }
    
    public WindowCreateWorkshopModDialogBuilder SetOnSelectMod(OnSelectMod onSelectMod)
    {
        OnSelectMod = onSelectMod;
        return this;
    }
    
    public WindowCreateWorkshopModDialogBuilder SetItemList(IEnumerable<ProjectListCreateWorkshopItem> itemList)
    {
        ItemList = itemList;
        return this;
    }
}