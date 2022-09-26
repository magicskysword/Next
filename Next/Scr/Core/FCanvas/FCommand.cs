using Fungus;
using Newtonsoft.Json;

namespace SkySwordKill.Next.FCanvas;

public class FCommand
{
    [JsonProperty(Order = -10)]
    public int ItemID;
    [JsonProperty(Order = -9)]
    public string CmdType;
    public virtual void ReadCommand(Command command)
    {
        CmdType = command.GetType().FullName;
        ItemID = command.ItemId;
    }

    public virtual Command WriteCommand()
    {
        return null;
    }
}