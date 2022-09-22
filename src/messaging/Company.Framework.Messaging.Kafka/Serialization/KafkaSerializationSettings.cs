using System.Text.Json;

namespace Company.Framework.Messaging.Kafka.Serialization;

public record KafkaSerializationSettings(JsonSerializerOptions JsonSerializerOptions);