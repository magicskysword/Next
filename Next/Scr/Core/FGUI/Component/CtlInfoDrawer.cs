using FairyGUI;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlInfoDrawer : CtlPropertyDrawerBase
    {
        public CtlInfoDrawer()
        {

        }

        private UI_ComInfoDrawer Drawer { get; set; }

        protected override GComponent OnCreateCom()
        {
            Drawer = UIPackage.CreateObject(FGUIManager.PKG_NEXT_CORE, "ComTextDrawer").As<UI_ComInfoDrawer>();
            return Drawer;
        }
        
        public void SetTitle(string title)
        {
            Drawer.title = title;
        }

        public void SetContent(string content)
        {
            Drawer.m_content.text = content;
        }
    }
}