using System;
using System.Collections.Generic;
using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component;

public interface IPropertyDrawer : IUndoInst
{
    bool Editable { get; set; }
    GComponent CreateCom();
    void RemoveCom();
    List<IPropertyDrawer> ChainDrawers { get; set; }
    void Refresh(bool chainRefresh = true);
    IPropertyDrawer AddChangeListener(Action OnDrawerChanged);
    IPropertyDrawer RemoveChangeListener(Action OnDrawerChanged);
    IPropertyDrawer ClearChangeListener();
    IPropertyDrawer AddChainDrawer(IPropertyDrawer iconDrawer);
    IPropertyDrawer RemoveChainDrawer(IPropertyDrawer iconDrawer);
}