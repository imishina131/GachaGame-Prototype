using GachaGame.Azure.Core.DataTypes;
using GachaGame.Azure.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GachaGame.Azure.Core.Helpers;
/// <summary>
/// A <see cref="JsonConverter{T}"/> that serializes and deserializes <see cref="IRollResolver{T}"/>
/// </summary>
/// <typeparam name="T">The type that the <see cref="IRollResolver{T}"/> resolves</typeparam>
public class RollResolverConverter<T> : JsonConverter<IRollResolver<T>> where T : IRollData
{
    /// <inheritdoc/>
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
        return (IRollResolver<T>?)jo.ToObject(resolverType, serializer) ?? new EmptyRollResolver<T>();
    }
    /// <inheritdoc/>
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

        JObject jo = JObject.FromObject(value, serializer);
        jo["$type"] = value.GetType().AssemblyQualifiedName;
        jo.WriteTo(writer);
    }
}