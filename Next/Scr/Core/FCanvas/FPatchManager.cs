using System;
using System.Collections.Generic;
using System.Linq;
using Fungus;

namespace SkySwordKill.Next.FCanvas
{
    public class FPatchManager
    {
        public Dictionary<string,List<FPatch>> PatchGroup = new Dictionary<string,List<FPatch>>();

        public void Init()
        {
            
        }
        
        public void Reset()
        {
            PatchGroup.Clear();
        }
        
        public void AddPatch(FPatch patch)
        {
            var group = patch.TargetFlowchart;
            if(!PatchGroup.ContainsKey(group))
            {
                PatchGroup.Add(group,new List<FPatch>());
            }
            PatchGroup[group].Add(patch);
        }
        
        public List<FPatch> GetAllPatch()
        {
            List<FPatch> result = new List<FPatch>();
            foreach (var item in PatchGroup)
            {
                result.AddRange(item.Value);
            }
            return result;
        }

        public List<FPatch> GetPatches(string flowchartName)
        {
            List<FPatch> result = new List<FPatch>();
            if (PatchGroup.ContainsKey(flowchartName))
            {
                result.AddRange(PatchGroup[flowchartName]);
            }

            return result;
        }

        public void PatchFlowchart(Flowchart flowchart)
        {
            var fPatches = GetPatches(flowchart.GetParentName());
            if(fPatches.Count == 0)
                return;
            if(flowchart.GetComponent<FPatchRecord>() != null)
                return;
            flowchart.gameObject.AddComponent<FPatchRecord>();
            var patches = fPatches.ToLookup(p => p.TargetBlock);
            foreach (var block in flowchart.GetComponents<Block>())
            {
                if (patches.Contains(block.ItemId))
                {
                    var fPatchCmdGroup = patches[block.ItemId].ToLookup(p => p.TargetCommand);
                    List<Command> removeCommand = new List<Command>();
                    for (var index = 0; index < block.CommandList.Count; index++)
                    {
                        var command = block.CommandList[index];
                        if (fPatchCmdGroup.Contains(command.ItemId))
                        {
                            var fPatchCmd = fPatchCmdGroup[command.ItemId].ToList();
                            fPatchCmd.Sort((a, b) => a.Priority.CompareTo(b.Priority));
                            foreach (var fPatch in fPatchCmd)
                            {
                                switch (fPatch.Type)
                                {
                                    case FPatch.PatchType.Insert:
                                        var pCommand = CreatePatchCommand(flowchart, block, fPatch);
                                        block.CommandList.Insert(index, pCommand);
                                        index++;
                                        break;
                                    case FPatch.PatchType.Delete:
                                        if (!removeCommand.Contains(command))
                                        {
                                            removeCommand.Add(command);
                                        }
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException(fPatch.Type.ToString());
                                }
                            }
                        }
                    }
                    if (fPatchCmdGroup.Contains(-1))
                    {
                        var fPatchCmd = fPatchCmdGroup[-1].ToList();
                        fPatchCmd.Sort((a, b) => -a.Priority.CompareTo(b.Priority));
                        foreach (var fPatch in fPatchCmd)
                        {
                            switch (fPatch.Type)
                            {
                                case FPatch.PatchType.Insert:
                                    var pCommand = CreatePatchCommand(flowchart, block, fPatch);
                                    block.CommandList.Add(pCommand);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(fPatch.Type.ToString());
                            }
                        }
                    }
                    // 移除对应CMD
                    foreach (var command in removeCommand)
                    {
                        block.CommandList.Remove(command);
                    }
                    // 重新修正CommandIndex
                    for (var index = 0; index < block.CommandList.Count; index++)
                    {
                        block.CommandList[index].CommandIndex = index;
                    }
                }
            }
            
            Main.LogInfo($"为{flowchart.GetParentName()} 添加Patch");
        }

        private PCommand CreatePatchCommand(Flowchart flowchart, Block block, FPatch fPatch)
        {
            var pCommand = FFlowchartTools.GetPCommandType(block.gameObject, fPatch.Command.CmdType);
            pCommand.Init(block, fPatch.Command);
            pCommand.ItemId = flowchart.NextItemId();
            return pCommand;
        }
    }
}