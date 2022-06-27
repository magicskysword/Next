using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SkySwordKill.Next.FCanvas
{
    public class FPatch
    {
        public enum PatchType
        {
            Insert,
            Delete
        }
        
        public string TargetFlowchart;
        public int TargetBlock;
        public int TargetCommand;
        public int Priority;
        [JsonConverter(typeof(StringEnumConverter))]
        public PatchType Type;
        public FPatchCommand Command;
    }
}