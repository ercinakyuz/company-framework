using Company.Framework.Core.Serialization;
using Company.Framework.Messaging.Sqs.Client.Context;
using Company.Framework.Messaging.Sqs.Consumer.Retrying.Handler;
using Company.Framework.Messaging.Sqs.Consumer.Settings;

namespace Company.Framework.Messaging.Sqs.Consumer.Context;

public interface ISqsConsumerContext
{
    IJsonSerializer JsonSerializer { get; }
    ISqsClientContext ClientContext { get; }
    SqsConsumerSettings Settings { get; }

    ISqsConsumerRetryingHandler? RetryingHandler { get; }


}