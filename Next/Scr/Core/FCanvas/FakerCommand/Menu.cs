﻿using Fungus;
using HarmonyLib;

namespace SkySwordKill.Next.FCanvas.FakerCommand;

[FCommandBinder(typeof(Fungus.Menu))]
public class Menu : FCommand
{
    public string Text { get; set; }
    public string TargetBlockID { get; set; }
        
    public override void ReadCommand(Command command)
    {
        base.ReadCommand(command);
        var cmdMenu = (Fungus.Menu)command;
            
        Text = (string)AccessTools.Field(cmdMenu.GetType(), "text").GetValue(command);
        var targetBlock = (Block)AccessTools.Field(cmdMenu.GetType(), "targetBlock").GetValue(command);
        if (targetBlock != null)
        {
            TargetBlockID = $"this:{targetBlock.ItemId}({targetBlock.blockName})";
        }
        else
        {
            TargetBlockID = "";
        }
    }

    public override string GetSummary()
    {
        return $"【选项】{Text} => {TargetBlockID}";
    }
}