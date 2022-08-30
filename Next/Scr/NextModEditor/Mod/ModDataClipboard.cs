using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.NextEditor.Mod
{
    public class CopyData
    {
        public string Name { get; set; }
        public IModData Data { get; set; }
        public ModProject Project { get; set; }
    }
    
    /// <summary>
    /// Mod管理器剪切板
    /// </summary>
    public class ModDataClipboard
    {
        public CopyData CurCopyData { get; set; }
    }
}