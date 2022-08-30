using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component
{
    public interface IPropertyDrawer
    {
        bool Editable { get; set; }
        GComponent CreateCom();
        void RemoveCom();
        void Refresh();
    }
}