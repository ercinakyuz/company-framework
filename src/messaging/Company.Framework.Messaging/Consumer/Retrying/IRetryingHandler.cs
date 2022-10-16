
using Company.Framework.Messaging.Consumer.Retrying.Args;

namespace Company.Framework.Messaging.Consumer.Retrying;

public interface IConsumerRetryingHandler<in TArgs>
{
    Task HandleAsync(TArgs args, CancellationToken cancellationToken);
}

public interface IConsumerRetryingHandler: IConsumerRetryingHandler<ConsumerRetrialArgs>
{
}