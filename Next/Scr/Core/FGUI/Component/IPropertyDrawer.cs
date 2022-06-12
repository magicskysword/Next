using FairyGUI;

namespace SkySwordKill.Next.FGUI.ComponentCtl
{
    public interface IPropertyDrawer
    {
        GComponent CreateCom();
        void RemoveCom();
        void Refresh();
    }
}