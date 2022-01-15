using System.Collections.Generic;
using System.Reflection;

namespace SkySwordKill.Next.Mod
{
    public class ModConfig
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public ModState State { get; set; }
        public string Path { get; set; }

        public Dictionary<string, string> jsonPathCache = new Dictionary<string, string>();
    }
}