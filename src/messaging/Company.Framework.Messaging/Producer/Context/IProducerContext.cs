namespace Company.Framework.Messaging.Producer.Context;

public interface IProducerContext
{
}

public interface IProducerContext<out TProducer> : IProducerContext where TProducer : IProducer
{
    TProducer Default();
    public TProducer Resolve(string name);
}