using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component
{
    public interface IPropertyDrawer
    {
        GComponent CreateCom();
        void RemoveCom();
        void Refresh();
    }
}