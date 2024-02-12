using Company.Framework.Core.Id.Abstractions;
using Company.Framework.Messaging.Kafka.Model;
using Company.Framework.Messaging.Producer.Args;

namespace Company.Framework.Messaging.Kafka.Producer.Args;

public record KafkaProduceArgs(string Topic, object Message, object? Headers = default) : CoreProduceArgs(Message);

public record KafkaProduceArgs<TId, TMessage>(TId Id, TMessage Message, KafkaHeaders? Headers = default, string? Topic = default);