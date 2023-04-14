using System.Runtime.Serialization;
using System.Text.Json;

namespace Company.Framework.Core.Serialization;

public class CoreJsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions _options;

    public CoreJsonSerializer(JsonSerializerOptions options)
    {
        _options = options;
    }

    public string Serialize(object value)
    {
        return JsonSerializer.Serialize(value, _options);
    }

    public MemoryStream SerializeToStream(object value)
    {
        var stream = new MemoryStream();
        JsonSerializer.Serialize(stream, value, _options);
        return stream;
    }

    public byte[] SerializeToUtf8Bytes(object value)
    {
        return JsonSerializer.SerializeToUtf8Bytes(value, _options);
    }

    public TValue Deserialize<TValue>(string json)
    {
        return JsonSerializer.Deserialize<TValue>(json, _options) ?? throw new SerializationException();
    }

    public TValue Deserialize<TValue>(byte[] utf8Json)
    {
        return JsonSerializer.Deserialize<TValue>(utf8Json, _options) ?? throw new SerializationException();
    }
}