using System;
using Newtonsoft.Json;
using UnityEngine;

public class Vector2Converter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        if (objectType == typeof(Vector2[]))
            objectType = typeof(Vector2Converter);
        return (objectType == typeof(Vector2) || objectType == typeof(Vector2[]));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        string[] v2Str = reader.Value.ToString().Split(',');
        v2Str[0] = v2Str[0].Replace("(", "");
        v2Str[1] = v2Str[1].Replace(")", "");
        return new Vector2(float.Parse(v2Str[0]), float.Parse(v2Str[1]));
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Vector2 v2 = (Vector2)value;
        serializer.Serialize(writer, "(" + v2.x + "," + v2.y + ")");
    }
}