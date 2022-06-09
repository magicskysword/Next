using FairyGUI;

namespace SkySwordKill.NextFGUI.NextCore
{
    public interface IPropertyDrawer
    {
        GComponent CreateCom();
        void RemoveCom();
        void Refresh();
    }
}