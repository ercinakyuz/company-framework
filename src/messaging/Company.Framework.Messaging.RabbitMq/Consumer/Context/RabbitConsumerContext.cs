using Company.Framework.Core.Serialization;
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
        public IJsonSerializer JsonSerializer { get; }

        public RabbitConsumerContext(IRabbitConnectionContext connectionContext, RabbitConsumerSettings settings, IJsonSerializer jsonSerializer, IRabbitConsumerRetryingHandler? retryingHandler = default)
        {
            ConnectionContext = connectionContext;
            Settings = settings;
            JsonSerializer = jsonSerializer;
            RetryingHandler = retryingHandler;
        }
    }
}
