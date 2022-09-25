using Company.Framework.Messaging.Producer.Args;

namespace Company.Framework.Messaging.Kafka.Producer.Args;

public record KafkaProduceArgs(string Topic, object Message) : CoreProduceArgs(Message);