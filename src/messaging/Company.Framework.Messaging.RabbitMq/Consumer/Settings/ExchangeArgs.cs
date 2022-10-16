namespace Company.Framework.Messaging.RabbitMq.Consumer.Settings;

public class RabbitExchangeArgs
{
    public string Name { get; init; }
    public string Type { get; init; }

    public void Deconstruct(out string Name, out string Type)
    {
        Name = this.Name;
        Type = this.Type;
    }
}