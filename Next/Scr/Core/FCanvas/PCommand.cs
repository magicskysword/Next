using Fungus;

namespace SkySwordKill.Next.FCanvas
{
    public class PCommand : Command
    {
        public void Init(Block parentBlock, FPatchCommand fPatchCommand)
        {
            ParentBlock = parentBlock;
            OnInit(fPatchCommand);
        }
        
        public virtual void OnInit(FPatchCommand fPatchCommand)
        {
            
        }
    }
}