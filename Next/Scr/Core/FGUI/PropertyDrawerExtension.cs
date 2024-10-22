using System.Collections.Generic;
using SkySwordKill.Next.FGUI.Component;

namespace SkySwordKill.Next.FGUI;

public static class PropertyDrawerExtension
{
    public static void RefreshWithChain(this IEnumerable<IPropertyDrawer> drawers)
    {
        var drawersHashSet = new HashSet<IPropertyDrawer>();
        var drawersQueue = new Queue<IPropertyDrawer>();
        
        foreach (var drawer in drawers)
        {
            drawersHashSet.Add(drawer);
            drawersQueue.Enqueue(drawer);
        }
        
        while (drawersQueue.Count > 0)
        {
            var currentDrawer = drawersQueue.Dequeue();
            currentDrawer.Refresh(false);
            
            if (currentDrawer.ChainDrawers != null)
            {
                foreach (var chainDrawer in currentDrawer.ChainDrawers)
                {
                    if (drawersHashSet.Add(chainDrawer))
                    {
                        drawersQueue.Enqueue(chainDrawer);
                    }
                }
            }
        }
    }
}