using Company.Framework.Messaging.Producer.Args;

namespace Company.Framework.Messaging.Producer;

public interface IProducer
{
    Task ProduceAsync<TMessage>(ProduceArgs<TMessage> args, CancellationToken cancellationToken) where TMessage : notnull;
}