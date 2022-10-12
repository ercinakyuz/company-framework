using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Model;

public class KafkaHeaders
{
    private readonly IDictionary<string, object> _headerDictionary;

    public KafkaHeaders()
    {
        _headerDictionary = new ConcurrentDictionary<string, object>();
    }
    public KafkaHeaders(IDictionary<string, object> headerDictionary)
    {
        _headerDictionary = headerDictionary;
    }

    public void Add(string key, object value)
    {
        _headerDictionary.TryAdd(key, value);
    }

    public object GetValue(string key)
    {
        return _headerDictionary[key];
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value)
    {
        return _headerDictionary.TryGetValue(key, out value);
    }

    internal static KafkaHeaders From(Headers headers)
    {
        return new KafkaHeaders(headers.ToDictionary(header => header.Key,
            header => JsonSerializer.Deserialize<object>(header.GetValueBytes())
                      ?? throw new SerializationException($"Could not deserialize header value for key: {header.Key}")));
    }

    internal static Headers? To(KafkaHeaders? kafkaHeaders)
    {
        if (kafkaHeaders == default || !kafkaHeaders._headerDictionary.Any())
            return default;

        var headers = new Headers();

        foreach (var keyValuePair in kafkaHeaders._headerDictionary)
        {
            var value = JsonSerializer.SerializeToUtf8Bytes(keyValuePair.Value);
            headers.Add(keyValuePair.Key, value);
        }

        return headers;
    }
}