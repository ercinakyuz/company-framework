using Company.Framework.Messaging.Bus;
using RabbitMQ.Client;

namespace Company.Framework.Messaging.RabbitMq.Bus;

public class RabbitBus : IBus
{
    public string Name { get; }

    public IModel Model { get; }

    public RabbitBus(string name, IModel model)
    {
        Name = name;
        Model = model;
    }
}