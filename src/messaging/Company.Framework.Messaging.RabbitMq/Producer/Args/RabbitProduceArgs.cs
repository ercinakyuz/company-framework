using Company.Framework.Messaging.Producer.Args;

namespace Company.Framework.Messaging.RabbitMq.Producer.Args;

public record RabbitProduceArgs(string Exchange, string Routing, object Message) : CoreProduceArgs(Message);