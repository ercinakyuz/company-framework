namespace Company.Framework.Messaging.Producer;

public interface IProducer<in TArgs> : IProducer
{
    Task ProduceAsync(TArgs args, CancellationToken cancellationToken);
}

public interface IProducer
{
    public string Name { get; }
    public string BusName { get; }
}