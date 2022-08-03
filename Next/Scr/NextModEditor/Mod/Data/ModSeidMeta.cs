using System.Collections.Generic;

namespace SkySwordKill.NextModEditor.Mod.Data
{
    public class ModSeidMeta : IModData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public List<ModSeidProperty> Properties { get; set; }
        public string IDName { get; set; }
    }
}