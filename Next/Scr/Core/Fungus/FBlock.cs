using System.Collections.Generic;
using Fungus;

namespace SkySwordKill.Next.FungusTools
{
    public class FBlock
    {
        public int ItemID;
        public string Name;
        public string Description;
        public List<FCommand> Commands = new List<FCommand>();


        public void ReadBlock(Block block)
        {
            ItemID = block.ItemId;
            Name = block.BlockName;
            Description = block.BlockName;
            
            foreach (var command in block.CommandList)
            {
                var fCmd = command.GetType().CreateBindFCommand();
                fCmd.ReadCommand(command);
                Commands.Add(fCmd);
            }
        }
    }
}