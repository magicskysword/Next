using FairyGUI;

namespace SkySwordKill.Next.FGUI
{
    public static class FGUITools
    {
        public static T As<T>(this GComponent component) where T : GComponent
        {
            return component as T;
        }
        
    }
}