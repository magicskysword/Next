using BepInEx.Configuration;

namespace SkySwordKill.Next
{
    public class ConfigTarget<T>
    {
        public ConfigEntry<T> configEntry;
        public ConfigDescription configDescription;
        
        internal ConfigurationManagerAttributes advancedSetting;

        public T Value
        {
            get => configEntry.Value;
            set => configEntry.Value = value;
        }

        public ConfigTarget(ConfigFile config,string section,string key,T defaultValue,string desc)
        {
            advancedSetting = new ConfigurationManagerAttributes();
            configDescription = new ConfigDescription(desc, null, advancedSetting);
            configEntry = config.Bind(section, key, defaultValue, configDescription);
        }

        public void SetName(string name)
        {
            advancedSetting.DispName = name;
        }

        public void SetDesc(string desc)
        {
            advancedSetting.Description = desc;
        }
    }
}