using System;
using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.Next.FGUI;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.Next.FGUI.Component;

public class ChangeIdUndoCommand : IUndoCommand
{
    private int _oldId;
    private int _newId;
    private IModData _modData;
    private Action<IModData, int> _onChangeId;
        
    public ChangeIdUndoCommand(IModData modData,int oldId, int newId, Action<IModData, int> onChangeId)
    {
        _modData = modData;
        _oldId = oldId;
        _newId = newId;
        _onChangeId = onChangeId;
    }
        
    public void Execute()
    {
        _onChangeId(_modData, _newId);
    }
        
    public void Undo()
    {
        _onChangeId(_modData, _oldId);
    }
}
    
public class SwiftIdUndoCommand : IUndoCommand
{
    private IModData _modDataA;
    private IModData _modDataB;
    private Action<IModData, IModData> _onSwiftId;
        
    public SwiftIdUndoCommand(IModData modDataA,IModData modDataB,Action<IModData, IModData> onChangeId)
    {
        _modDataA = modDataA;
        _modDataB = modDataB;
        _onSwiftId = onChangeId;
    }
        
    public void Execute()
    {
        _onSwiftId(_modDataA, _modDataB);
    }
        
    public void Undo()
    {
        _onSwiftId(_modDataB, _modDataA);
    }
}
    
public class CtlIDPropertyDrawer : CtlPropertyDrawerBase
{
    private string _drawerName;
    private UI_ComNumberDrawer Drawer => (UI_ComNumberDrawer)Component;
    private Func<IEnumerable<IModData>> _dataListGetter;
    private Action<int> _onChangeID;
    private IModData _modData;

    public IEnumerable<IModData> DataList => _dataListGetter.Invoke();

    public CtlIDPropertyDrawer(string drawerName,
        IModData modData,
        Func<IEnumerable<IModData>> dataListGetter,
        Func<IModData, string> dataTitleGetter,
        Action<IModData, int> onChangeId,
        Action<IModData, IModData> onSwiftId,
        Action onCancel)
    {
        _drawerName = drawerName;
        _dataListGetter = dataListGetter;
        _modData = modData;

        _onChangeID = num =>
        {
            if (num == modData.Id)
                return;

            var otherData = DataList.FirstOrDefault(data => data.Id == num && data != modData);

            if (otherData != null)
            {
                WindowConfirmDialog.CreateDialog("提示",
                    $"已经存在ID为 {num} 的数据，是否交换 {dataTitleGetter.Invoke(modData)} 与 {dataTitleGetter.Invoke(otherData)} ID？",
                    true,
                    () =>
                    {
                        this.Record(new SwiftIdUndoCommand(modData, otherData, onSwiftId));
                        OnChanged?.Invoke();
                    },
                    () => { onCancel?.Invoke(); });
            }
            else
            {
                WindowConfirmDialog.CreateDialog(
                    "提示",
                    $"即将把 {dataTitleGetter.Invoke(modData)} 的ID修改为 {num}，是否继续？",
                    true,
                    () =>
                    {
                        this.Record(new ChangeIdUndoCommand(modData, modData.Id, num, onChangeId));
                        OnChanged?.Invoke();
                    },
                    () => { onCancel?.Invoke(); });
            }
        };
    }

    protected override GComponent OnCreateCom()
    {
        var drawer = UI_ComNumberDrawer.CreateInstance();
        drawer.BindIntEndEdit(_onChangeID);
        drawer.m_inContent.text = _modData.Id.ToString();
        drawer.title = _drawerName;
        return drawer;
    }

    protected override void OnRefresh()
    {
        Drawer.m_inContent.text = _modData.Id.ToString();
    }
        
    protected override void SetDrawerEditable(bool value)
    {
        Drawer.SetEditable(value);
    }
}