using FairyGUI;

namespace SkySwordKill.Next.FGUI.Component
{
    public class CtlGroupInspector : CtlInspectorBase
    {
        public CtlGroupInspector(GList tagList)
        {
            MainView = tagList;
        }
        
        public GList MainView;

        protected override GList _drawerGList => MainView;

        protected override void OnRefresh()
        {
            if(MainView.visible)
                MainView.ResizeToFit();
            else
                MainView.height = 0;
        }

        public void Show()
        {
            MainView.visible = true;
            Refresh();
        }

        public void Hide()
        {
            MainView.visible = false;
            Refresh();
        }
    }
}