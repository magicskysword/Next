using FairyGUI;

namespace SkySwordKill.Next.FGUI
{
    public static class FGUITools
    {
        public static T As<T>(this GObject obj) where T : GObject
        {
            //Main.LogDebug($"Type : {component.GetType()} to Type : {typeof(T)}");
            return obj as T;
        }
        
    }
}