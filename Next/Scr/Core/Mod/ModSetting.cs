﻿using Newtonsoft.Json;

namespace SkySwordKill.Next.Mod;

public class ModSetting
{
    public int priority = 0;
    public bool enable = false;
    [JsonIgnore]
    public ModConfig BindMod { get; set; }
}