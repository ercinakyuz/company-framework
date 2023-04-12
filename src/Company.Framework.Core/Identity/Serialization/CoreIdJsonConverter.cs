using System.Text.Json;
using System.Text.Json.Serialization;

namespace Company.Framework.Core.Identity.Serialization;

public class CoreIdJsonConverter<TId, TValue> : JsonConverter<TId> where TId : CoreId<TId, TValue>
{
    private readonly JsonConverter<TValue> _valueConverter;


    public CoreIdJsonConverter(JsonSerializerOptions options)
    {
        _valueConverter = (JsonConverter<TValue>)options.GetConverter(typeof(TValue));
    }

    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = _valueConverter.Read(ref reader, typeof(TValue), options)!;
        return CoreId<TId, TValue>.From(value);
    }

    public override void Write(Utf8JsonWriter writer, TId id, JsonSerializerOptions options)
    {
        _valueConverter.Write(writer, id.Value!, options);
    }
}