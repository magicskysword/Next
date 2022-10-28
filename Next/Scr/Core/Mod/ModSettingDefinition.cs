using Newtonsoft.Json;
using SkySwordKill.Next.FGUI.Component;

namespace SkySwordKill.Next.Mod;


public abstract class ModSettingDefinition
{
    [JsonProperty]
    public abstract string Type { get; }
    [JsonProperty]
    public string Key { get; set; }
    [JsonProperty]
    public string Name { get; set; }
    [JsonProperty]
    public string Description { get; set; }
    
    public abstract void OnInit();
    public abstract void OnDrawer(IInspector inspector);
    
    public virtual void InitBool(string key, bool defaultValue)
    {
        if (!ModManager.TryGetModSetting(Key, out bool _))
        {
            ModManager.SetModSetting(Key, defaultValue);
        }
    }
    
    public virtual void InitLong(string key, long defaultValue)
    {
        if (!ModManager.TryGetModSetting(Key, out long _))
        {
            ModManager.SetModSetting(Key, defaultValue);
        }
    }
    
    public virtual void InitString(string key, string defaultValue)
    {
        if (!ModManager.TryGetModSetting(Key, out string _))
        {
            ModManager.SetModSetting(Key, defaultValue);
        }
    }
    
    public virtual void InitDouble(string key, double defaultValue)
    {
        if (!ModManager.TryGetModSetting(Key, out double _))
        {
            ModManager.SetModSetting(Key, defaultValue);
        }
    }
}