using System.Collections.Generic;
using Newtonsoft.Json;

namespace SkySwordKill.Next.StaticFace;

public class CustomStaticFaceInfo
{
    [JsonProperty(Order = 0)]
    public int ID { get; set; }
    [JsonProperty(Order = 1)]
    public Dictionary<string, int> RandomInfos = new Dictionary<string, int>();

    public int GetRandomInfo(string type)
    {
        if (RandomInfos.ContainsKey(type))
            return RandomInfos[type];
        Main.LogWarning($"StaticFace [{ID}] 缺失 {type} 类型数据。");
        return -100;
    }
}