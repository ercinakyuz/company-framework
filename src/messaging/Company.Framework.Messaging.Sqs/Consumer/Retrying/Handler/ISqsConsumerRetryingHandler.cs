using Company.Framework.Messaging.Consumer.Retrying;

namespace Company.Framework.Messaging.Sqs.Consumer.Retrying.Handler;

public interface ISqsConsumerRetryingHandler : IConsumerRetryingHandler
{
    string Queue { get; }
}