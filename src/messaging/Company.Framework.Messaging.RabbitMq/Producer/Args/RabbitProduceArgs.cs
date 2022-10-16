using Company.Framework.Messaging.Producer.Args;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;

namespace Company.Framework.Messaging.RabbitMq.Producer.Args;

public record RabbitProduceArgs(RabbitExchangeArgs Exchange, string Routing, object Message, IDictionary<string, object>? Headers = default) : CoreProduceArgs(Message);