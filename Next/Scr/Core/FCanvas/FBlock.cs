using System.Collections.Generic;
using Fungus;
using HarmonyLib;
using Newtonsoft.Json;
using UnityEngine;

namespace SkySwordKill.Next.FCanvas
{
    public class FBlock
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(Vector2Converter))]
        public Vector2 Position { get; set; }
        public List<FCommand> Commands { get; set; } = new List<FCommand>();


        public void ReadBlock(Block block)
        {
            ItemID = block.ItemId;
            Name = block.BlockName;
            Description = block.BlockName;
            var rect = (Rect)AccessTools.Field(typeof(Block),"nodeRect").GetValue(block);
            Position = rect.position;
            
            foreach (var command in block.CommandList)
            {
                var fCmd = command.GetType().CreateBindFCommand();
                fCmd.ReadCommand(command);
                Commands.Add(fCmd);
            }
        }
    }
}