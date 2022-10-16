using Company.Framework.Messaging.RabbitMq.Connection.Context;
using Company.Framework.Messaging.RabbitMq.Consumer.Retrying.Handler;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;

namespace Company.Framework.Messaging.RabbitMq.Consumer.Context;

public interface IRabbitConsumerContext
{
    IRabbitConnectionContext ConnectionContext { get; }
    RabbitConsumerSettings Settings { get; }

    IRabbitConsumerRetryingHandler? RetryingHandler { get; }

}