using Company.Framework.Messaging.Consumer.Retrying;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;

namespace Company.Framework.Messaging.RabbitMq.Consumer.Retrying.Handler;

public interface IRabbitConsumerRetryingHandler : IConsumerRetryingHandler
{
    RabbitDeclarationArgs DeclarationArgs { get; }
}