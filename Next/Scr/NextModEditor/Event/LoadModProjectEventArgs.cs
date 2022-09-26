using SkySwordKill.NextModEditor.Mod;

namespace SkySwordKill.NextModEditor.Event;

public class LoadModProjectEventArgs : EventArgs
{
    public ModProject ModProject { get; set; }
}