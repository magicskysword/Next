using Newtonsoft.Json;

namespace SkySwordKill.Next.Mod;

public class ModGroupSetting
{
    public int priority = 0;
    [JsonIgnore]
    public ModGroup BindGroup { get; set; }
}