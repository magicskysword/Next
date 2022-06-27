using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SkySwordKill.Next.FCanvas
{
    public class FCommandConverter: JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            // Using a nullable bool here in case "is_album" is not present on an item
            string cmdType = jo["CmdType"]?.ToString();

            FCommand command;
            if (string.IsNullOrEmpty(cmdType))
            {
                command = new FCommand();
            }
            else
            {
                command = FFlowchartTools.CreateFCommand(cmdType);
            }

            serializer.Populate(jo.CreateReader(), command);

            return command;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(FCommand).IsAssignableFrom(objectType);
        }
    }
}