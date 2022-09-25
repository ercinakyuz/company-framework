namespace Company.Framework.Messaging.Bus;

public abstract class CoreBus : IBus
{
    public string Name { get; }

    protected CoreBus(string name)
    {
        Name = name;
    }
}