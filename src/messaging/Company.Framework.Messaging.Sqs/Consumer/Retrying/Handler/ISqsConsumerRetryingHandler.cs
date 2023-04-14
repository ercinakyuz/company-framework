using Company.Framework.Messaging.Consumer.Retrying;
using Company.Framework.Messaging.Sqs.Consumer.Settings;

namespace Company.Framework.Messaging.Sqs.Consumer.Retrying.Handler;

public interface ISqsConsumerRetryingHandler : IConsumerRetryingHandler
{
    SqsConsumerSettings ConsumerSettings { get; }
}