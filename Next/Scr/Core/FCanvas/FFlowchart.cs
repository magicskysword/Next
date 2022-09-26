using System.Collections.Generic;
using Fungus;

namespace SkySwordKill.Next.FCanvas;

public class FFlowchart
{
    public string Name;
        
    public List<FBlock> Blocks = new List<FBlock>();

    public List<FVariable> Variables = new List<FVariable>();

    public void ReadFlowchart(Flowchart flowchart)
    {
        Name = flowchart.GetParentName();
            
        foreach (var block in flowchart.GetComponents<Block>())
        {
            var fBlock = new FBlock();
            fBlock.ReadBlock(block);
            Blocks.Add(fBlock);
        }

        foreach (var variable in flowchart.Variables)
        {
                
        }
    }
}