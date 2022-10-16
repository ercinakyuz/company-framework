using Company.Framework.Messaging.RabbitMq.Connection.Context;
using Company.Framework.Messaging.RabbitMq.Consumer.Retrying.Handler;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;

namespace Company.Framework.Messaging.RabbitMq.Consumer.Context
{
    public class RabbitConsumerContext: IRabbitConsumerContext
    {
        public IRabbitConnectionContext ConnectionContext { get; }
        public RabbitConsumerSettings Settings { get; }

        public IRabbitConsumerRetryingHandler? RetryingHandler { get; }

        public RabbitConsumerContext(IRabbitConnectionContext connectionContext, RabbitConsumerSettings settings, IRabbitConsumerRetryingHandler? retryingHandler = default)
        {
            ConnectionContext = connectionContext;
            Settings = settings;
            RetryingHandler = retryingHandler;
        }
    }
}
