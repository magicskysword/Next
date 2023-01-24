using System;
using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component;

public interface IPropertyDrawer : IUndoInst
{
    bool Editable { get; set; }
    GComponent CreateCom();
    void RemoveCom();
    void Refresh();
    IPropertyDrawer AddChangeListener(Action onChanged);
    IPropertyDrawer RemoveChangeListener(Action onChanged);
    IPropertyDrawer ClearChangeListener();
    IPropertyDrawer AddChainDrawer(IPropertyDrawer iconDrawer);
}