using Company.Framework.Messaging.Producer.Args;

namespace Company.Framework.Messaging.Producer;

public interface IProducer<in TArgs> : IProducer where TArgs : CoreProduceArgs
{
    Task ProduceAsync(TArgs args, CancellationToken cancellationToken);
}

public interface IProducer
{
    public string Name { get; }
    public string BusName { get; }
}