using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SkySwordKill.Next.DialogSystem;

public class AvatarNextData
{
    public DataGroup<int> IntGroup { get; set; } = new DataGroup<int>();
    public DataGroup<string> StrGroup { get; set; } = new DataGroup<string>();
}