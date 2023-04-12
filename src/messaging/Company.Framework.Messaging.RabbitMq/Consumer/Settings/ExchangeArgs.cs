namespace Company.Framework.Messaging.RabbitMq.Consumer.Settings;

public class RabbitExchangeArgs
{
    public required string Name { get; init; }
    public required string Type { get; init; }

    public void Deconstruct(out string name, out string type)
    {
        name = Name;
        type = Type;
    }
}