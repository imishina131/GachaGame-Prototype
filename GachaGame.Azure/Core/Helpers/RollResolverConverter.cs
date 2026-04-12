using GachaGame.Azure.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GachaGame.Azure.Core.Helpers;

public class RollResolverConverter<T> : JsonConverter<IRollResolver<T>> where T : struct, IRollData
{
    public override IRollResolver<T> ReadJson(
        JsonReader reader,
        Type objectType,
        IRollResolver<T>? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return new EmptyRollResolver<T>();
        JObject jo = JObject.Load(reader);
        string? typeName = jo["$type"]?.Value<string>();
        if (typeName is null) return new EmptyRollResolver<T>();
        Type? resolverType = Type.GetType(typeName);
        if (resolverType is null || !typeof(IRollResolver<T>).IsAssignableFrom(resolverType)) return new EmptyRollResolver<T>();
        return (IRollResolver<T>?)Activator.CreateInstance(resolverType) ?? new EmptyRollResolver<T>();
    }

    public override void WriteJson(
        JsonWriter writer,
        IRollResolver<T>? value,
        JsonSerializer serializer)
    {
        if (value is null or EmptyRollResolver<T>)
        {
            writer.WriteNull();
            return;
        }

        JObject jo = new()
        {
            ["$type"] = value.GetType().AssemblyQualifiedName
        };
        jo.WriteTo(writer);
    }
}