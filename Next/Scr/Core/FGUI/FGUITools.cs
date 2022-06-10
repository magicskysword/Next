using FairyGUI;

namespace SkySwordKill.Next.FGUI
{
    public static class FGUITools
    {
        public static T As<T>(this GComponent component) where T : GComponent
        {
            //Main.LogDebug($"Type : {component.GetType()} to Type : {typeof(T)}");
            return component as T;
        }
        
    }
}