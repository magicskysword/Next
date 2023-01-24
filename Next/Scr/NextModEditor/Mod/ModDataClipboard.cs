using System.Collections.Generic;
using System.Linq;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.NextModEditor.Mod;

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
    public CopyData CurCopyData => CopyDatas.FirstOrDefault();
    public List<CopyData> CopyDatas { get; } = new List<CopyData>();
    
    public void SetCopyData(CopyData copyData)
    {
        CopyDatas.Clear();
        CopyDatas.Add(copyData);
    }
    
    public void SetCopyData(IEnumerable<CopyData> copyDatas)
    {
        CopyDatas.Clear();
        CopyDatas.AddRange(copyDatas);
    }
}