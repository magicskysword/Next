using System;
using System.Collections.Generic;
using Fungus;
using Newtonsoft.Json.Linq;

namespace SkySwordKill.Next.FCanvas
{
    public class FPatch
    {
        public string ID;
        public string FlowchartID;
        public int BlockID;
        public int CommandID;
        public JArray Commands;

        public List<Command> GetFCommands()
        {
            var list = new List<Command>();
            foreach (var jToken in Commands)
            {
                var jObject = (JObject)jToken;
                if (jObject == null)
                {
                    Main.LogWarning($"检测到FPatch {ID} : {FlowchartID}/{BlockID}/{CommandID} Patch存在空指令！");
                    continue;
                }
                
                var fType = jObject.GetValue("CmdType")?.Value<string>() ?? typeof(FCommand).FullName;
                var fCommand = (FCommand)jObject.ToObject(Type.GetType(fType).GetFCommandType());
                var cmd = fCommand.WriteCommand();
                if (cmd == null)
                {
                    Main.LogWarning($"检测到FPatch {ID} : {FlowchartID}/{BlockID}/{CommandID} Patch 指令类型 {fType} 不存在！");
                    continue;
                }
                list.Add(cmd);
            }
            return list;
        }
    }
}