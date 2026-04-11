using System.Reflection;
using GachaGame.Azure.Core.Interfaces;
using Newtonsoft.Json;

namespace GachaGame.Azure.Core.Helpers;

public class RollResolverConverter<T> : JsonConverter<IRollResolver<T>> where T : IRollData
{
    public override IRollResolver<T> ReadJson(JsonReader reader, Type objectType, IRollResolver<T>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value is not string typeName) throw new JsonException("Resolver name cannot be null");

        Type? type = Assembly.GetExecutingAssembly()
            .GetTypes()
            .FirstOrDefault(t => t.Name == typeName && typeof(IRollResolver<T>).IsAssignableFrom(t));
        if (type is null) throw new JsonException($"No resolver found with name {typeName}");
        return (IRollResolver<T>)Activator.CreateInstance(type)!;
    }

    public override void WriteJson(JsonWriter writer, IRollResolver<T>? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.GetType().Name);
    }
}