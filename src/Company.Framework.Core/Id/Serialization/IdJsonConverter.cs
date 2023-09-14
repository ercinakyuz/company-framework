using Company.Framework.Core.Id.Abstractions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Company.Framework.Core.Id.Serialization;

public class IdJsonConverter<TId, TValue> : JsonConverter<TId> where TId : IId<TId, TValue>
{
    private readonly JsonConverter<TValue> _valueConverter;


    public IdJsonConverter(JsonSerializerOptions options)
    {
        _valueConverter = (JsonConverter<TValue>)options.GetConverter(typeof(TValue));
    }

    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = _valueConverter.Read(ref reader, typeof(TValue), options)!;
        return TId.From(value);
    }

    public override void Write(Utf8JsonWriter writer, TId id, JsonSerializerOptions options)
    {
        _valueConverter.Write(writer, id.Value!, options);
    }
}