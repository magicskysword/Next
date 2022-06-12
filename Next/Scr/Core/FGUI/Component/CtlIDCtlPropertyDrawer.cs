using System;
using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using SkySwordKill.NextModEditor.Mod.Data;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.ComponentCtl
{
    public class CtlIDCtlPropertyDrawer : CtlPropertyDrawerBase
{
    private string _drawerName;
    private UI_ComIntDrawer Drawer => (UI_ComIntDrawer)Component;
    private Func<IEnumerable<IModData>> _dataListGetter;
    private Action<int> _onChangeID;
    private IModData _modData;

    public IEnumerable<IModData> DataList => _dataListGetter.Invoke();

    public CtlIDCtlPropertyDrawer(string drawerName,
        IModData modData,
        Func<IEnumerable<IModData>> dataListGetter,
        Func<IModData,string> dataTitleGetter,
        Action<IModData,int> onChangeId,
        Action<IModData,IModData> onSwiftId,
        Action onCancel)
    {
        _drawerName = drawerName;
        _dataListGetter = dataListGetter;
        _modData = modData;
            
        _onChangeID = num =>
        {
            if (num == modData.ID)
                return;

            var otherData = DataList.FirstOrDefault(data => data.ID == num && data != modData);

            if (otherData != null)
            {
                WindowConfirmDialog.CreateDialog("提示",
                    $"已经存在ID为 {num} 的数据，是否交换 {dataTitleGetter.Invoke(modData)} 与 {dataTitleGetter.Invoke(otherData)} ID？",
                    () =>
                    {
                        onSwiftId?.Invoke(modData, otherData);
                        OnChanged?.Invoke();
                    },
                    () =>
                    {
                        onCancel?.Invoke();
                    });
            }
            else
            {
                WindowConfirmDialog.CreateDialog(
                    "提示",
                    $"即将把 {dataTitleGetter.Invoke(modData)} 的ID修改为 {num}，是否继续？",
                    () =>
                    {
                        onChangeId?.Invoke(modData, num);
                        OnChanged?.Invoke();
                    },
                    () =>
                    {
                        onCancel?.Invoke();
                    });
            }
        };
    }
    
    protected override GComponent OnCreateCom()
    {
        var drawer = UI_ComIntDrawer.CreateInstance();
        drawer.BindEndEdit(_onChangeID);
        drawer.m_inContent.text = _modData.ID.ToString();
        drawer.title = _drawerName;
        return drawer;
    }

    protected override void OnRefresh()
    {
        Drawer.m_inContent.text = _modData.ID.ToString();
    }
}
}